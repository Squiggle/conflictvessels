# Reactive UI Patterns for ConflictVessels.Web

## Overview

This document establishes patterns for implementing Blazor components that leverage System.Reactive observables from the game engine. The goal is to create a **declarative, subscription-based UI** where components react to state changes automatically, rather than manually invoking state updates.

## Core Philosophy

**❌ ANTI-PATTERN: Manual State Updates**
```csharp
// BAD: Manually calling StateHasChanged on events
GameService.OnGameStateChanged += OnGameStateChanged;

private void OnGameStateChanged()
{
    InvokeAsync(() =>
    {
        LoadGrids();
        StateHasChanged();
    });
}
```

**✅ PATTERN: Reactive State Updates**
```csharp
// GOOD: Subscribing to observables and letting Rx manage updates
game.ObservablePhase
    .ObserveOn(SynchronizationContext.Current!)
    .Subscribe(phase =>
    {
        currentPhase = phase;
        StateHasChanged();
    });
```

## Architecture Layers

### Layer 1: Engine (Domain)
- **Responsibility**: Business logic and state management
- **Technology**: System.Reactive (`BehaviorSubject<T>`, `IObservable<T>`)
- **Observables Available**:
  - `Game.ObservablePhase` → `IObservable<GamePhase>`
  - `Arena.ObservableReady` → `IObservable<bool>`
  - `Grid.ObservableReady` → `IObservable<bool>`

### Layer 2: Services (Application)
- **Responsibility**: Expose engine observables to UI components
- **Pattern**: Service wraps engine entities and provides:
  - Direct access to engine observables (preferred)
  - Transformed observables for UI-specific needs
  - Command methods that modify engine state

### Layer 3: Components (Presentation)
- **Responsibility**: Subscribe to observables and render UI
- **Pattern**: Components subscribe in `OnInitialized`, update local state in subscription handlers, dispose subscriptions in `Dispose`

## Implementation Guidelines

### 1. Service Design Pattern

Services should expose observables directly, not wrap them in events:

```csharp
public class GameService : IDisposable
{
    private Game? game;
    private readonly List<IDisposable> subscriptions = [];

    // ✅ GOOD: Expose the game instance so components can access observables
    public Game? CurrentGame => game;

    // ✅ GOOD: Expose engine entities with their observables
    public Arena? CurrentArena => game?.Arena;

    // ✅ ACCEPTABLE: Expose transformed observables for complex UI scenarios
    public IObservable<GameViewModel> ObservableGameView =>
        game?.ObservablePhase
            .Select(phase => new GameViewModel(game, phase))
        ?? Observable.Empty<GameViewModel>();

    // ❌ BAD: Don't wrap observables in events
    // public event Action? OnGameStateChanged;

    public void StartNewGame()
    {
        DisposeCurrentGame();
        game = CreateGame();

        // Only subscribe internally if the service needs to maintain its own state
        // Otherwise, let components subscribe directly to game.ObservablePhase
    }

    private void DisposeCurrentGame()
    {
        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
        subscriptions.Clear();

        if (game is IDisposable disposable)
        {
            disposable.Dispose();
        }

        game = null;
    }

    public void Dispose()
    {
        DisposeCurrentGame();
    }
}
```

### 2. Component Subscription Pattern

Components should subscribe to observables in `OnInitialized` and manage subscriptions properly:

```csharp
@page "/game"
@using System.Reactive.Linq
@inject GameService GameService
@implements IDisposable

<div class="game-container">
    @if (GameService.CurrentGame == null)
    {
        <button @onclick="StartGame">Start New Game</button>
    }
    else
    {
        <div class="phase-indicator phase-@currentPhase.ToString().ToLower()">
            Phase: @currentPhase
        </div>

        <GridComponent Grid="player1Grid" />
        <GridComponent Grid="player2Grid" />
    }
</div>

@code {
    private readonly List<IDisposable> subscriptions = [];
    private GamePhase currentPhase = GamePhase.Setup;
    private Grid? player1Grid;
    private Grid? player2Grid;

    protected override void OnInitialized()
    {
        // Subscribe to game state when component initializes
        SubscribeToGameState();
    }

    private void StartGame()
    {
        GameService.StartNewGame();

        // Re-subscribe after creating a new game
        SubscribeToGameState();
    }

    private void SubscribeToGameState()
    {
        // Clear existing subscriptions
        foreach (var sub in subscriptions)
        {
            sub.Dispose();
        }
        subscriptions.Clear();

        var game = GameService.CurrentGame;
        if (game == null) return;

        // Subscribe to game phase changes
        var phaseSub = game.ObservablePhase
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(phase =>
            {
                currentPhase = phase;
                StateHasChanged();
            });
        subscriptions.Add(phaseSub);

        // Subscribe to arena ready state
        var arenaSub = game.Arena.ObservableReady
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(ready =>
            {
                // Update UI based on arena readiness
                StateHasChanged();
            });
        subscriptions.Add(arenaSub);

        // Load initial grid state
        player1Grid = game.Arena.Grids[0];
        player2Grid = game.Arena.Grids[1];

        StateHasChanged();
    }

    public void Dispose()
    {
        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
        subscriptions.Clear();
    }
}
```

### 3. Threading and Synchronization Context

**CRITICAL**: Blazor components run on the UI synchronization context. Observable subscriptions must marshal back to this context:

```csharp
// ✅ ALWAYS use ObserveOn to return to UI thread
game.ObservablePhase
    .ObserveOn(SynchronizationContext.Current!)
    .Subscribe(phase =>
    {
        currentPhase = phase;
        StateHasChanged(); // Safe: we're on UI thread
    });

// ❌ DANGEROUS: Direct subscription may execute on background thread
game.ObservablePhase
    .Subscribe(phase =>
    {
        currentPhase = phase;
        StateHasChanged(); // May throw: wrong thread
    });
```

### 4. Subscription Lifecycle Management

**Pattern**: Track all subscriptions and dispose them properly:

```csharp
@implements IDisposable

@code {
    private readonly List<IDisposable> subscriptions = [];

    protected override void OnInitialized()
    {
        var sub1 = observable1.Subscribe(...);
        var sub2 = observable2.Subscribe(...);

        subscriptions.Add(sub1);
        subscriptions.Add(sub2);
    }

    public void Dispose()
    {
        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
        subscriptions.Clear();
    }
}
```

**IMPORTANT**: Always dispose subscriptions to prevent memory leaks.

### 5. Derived State with Rx Operators

Use Rx operators to derive UI state instead of manual methods:

```csharp
// ❌ BAD: Manual polling/refresh
private void LoadGrids()
{
    player1Grid = GameService.GetPlayerGrid(0);
    player2Grid = GameService.GetPlayerGrid(1);
    StateHasChanged();
}

// ✅ GOOD: Derive from observable
private Grid? player1Grid;
private Grid? player2Grid;

protected override void OnInitialized()
{
    var game = GameService.CurrentGame;
    if (game == null) return;

    // Grids are immutable after game creation for current design
    player1Grid = game.Arena.Grids[0];
    player2Grid = game.Arena.Grids[1];

    // If grids could change, subscribe to an observable that provides them
    var gridSub = game.Arena.ObservableGrids // (if this existed)
        .ObserveOn(SynchronizationContext.Current!)
        .Subscribe(grids =>
        {
            player1Grid = grids[0];
            player2Grid = grids[1];
            StateHasChanged();
        });
    subscriptions.Add(gridSub);
}
```

### 6. Combining Multiple Observables

Use `CombineLatest` or `Zip` for state that depends on multiple observables:

```csharp
var combinedSub = Observable.CombineLatest(
    game.ObservablePhase,
    game.Arena.ObservableReady,
    (phase, ready) => new { Phase = phase, Ready = ready }
)
.ObserveOn(SynchronizationContext.Current!)
.Subscribe(state =>
{
    currentPhase = state.Phase;
    isReady = state.Ready;
    StateHasChanged();
});

subscriptions.Add(combinedSub);
```

### 7. Conditional Observables

Handle nullable observables gracefully:

```csharp
// ✅ GOOD: Safe observable access with fallback
public IObservable<GamePhase> ObservablePhase =>
    GameService.CurrentGame?.ObservablePhase
    ?? Observable.Return(GamePhase.Setup);

// ✅ GOOD: Switch to new observable when source changes
var gameSwitchSub = GameService.ObservableCurrentGame // (if this existed)
    .Select(game => game?.ObservablePhase ?? Observable.Empty<GamePhase>())
    .Switch()
    .ObserveOn(SynchronizationContext.Current!)
    .Subscribe(phase =>
    {
        currentPhase = phase;
        StateHasChanged();
    });
```

## Common Patterns

### Pattern A: Simple State Display

**Scenario**: Display a single piece of state that updates reactively

```csharp
@page "/game-status"
@inject GameService GameService
@implements IDisposable

<div>Current Phase: @currentPhase</div>

@code {
    private readonly List<IDisposable> subscriptions = [];
    private GamePhase currentPhase = GamePhase.Setup;

    protected override void OnInitialized()
    {
        var game = GameService.CurrentGame;
        if (game == null) return;

        var sub = game.ObservablePhase
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(phase =>
            {
                currentPhase = phase;
                StateHasChanged();
            });
        subscriptions.Add(sub);
    }

    public void Dispose()
    {
        foreach (var sub in subscriptions) sub.Dispose();
        subscriptions.Clear();
    }
}
```

### Pattern B: List Rendering with Observable Collection

**Scenario**: Display a dynamic list that updates automatically

```csharp
@inject GameService GameService
@implements IDisposable

<ul>
    @foreach (var action in playerActions)
    {
        <li>@action</li>
    }
</ul>

@code {
    private readonly List<IDisposable> subscriptions = [];
    private readonly List<string> playerActions = [];

    protected override void OnInitialized()
    {
        var game = GameService.CurrentGame;
        if (game?.PlayerActions == null) return;

        // Subscribe to ObservableCollection changes
        game.PlayerActions.CollectionChanged += (s, e) =>
        {
            InvokeAsync(() =>
            {
                // Sync local list with observable collection
                playerActions.Clear();
                playerActions.AddRange(game.PlayerActions);
                StateHasChanged();
            });
        };
    }

    public void Dispose()
    {
        foreach (var sub in subscriptions) sub.Dispose();
        subscriptions.Clear();
    }
}
```

### Pattern C: Nested Component Communication

**Scenario**: Parent component passes observables to child components

```csharp
// Parent.razor
<GridComponent GridObservable="@gridObservable" />

@code {
    private IObservable<Grid>? gridObservable;

    protected override void OnInitialized()
    {
        var game = GameService.CurrentGame;
        if (game == null) return;

        // Create an observable that emits the grid whenever relevant state changes
        gridObservable = game.Arena.ObservableReady
            .Select(_ => game.Arena.Grids[0])
            .ObserveOn(SynchronizationContext.Current!);
    }
}

// GridComponent.razor
@implements IDisposable

@code {
    [Parameter]
    public IObservable<Grid>? GridObservable { get; set; }

    private readonly List<IDisposable> subscriptions = [];
    private Grid? currentGrid;

    protected override void OnInitialized()
    {
        if (GridObservable == null) return;

        var sub = GridObservable
            .Subscribe(grid =>
            {
                currentGrid = grid;
                StateHasChanged();
            });
        subscriptions.Add(sub);
    }

    public void Dispose()
    {
        foreach (var sub in subscriptions) sub.Dispose();
        subscriptions.Clear();
    }
}
```

### Pattern D: Command/Event Handling

**Scenario**: User actions trigger engine commands, state updates flow back via observables

```csharp
@code {
    private void HandleCellClick(Coords coords)
    {
        var game = GameService.CurrentGame;
        if (game == null || game.Phase != GamePhase.Action) return;

        // Execute command (will mutate engine state)
        game.Attack(coords); // (when implemented)

        // State updates will flow back through observables automatically
        // No need to call StateHasChanged() here!
    }
}
```

## Anti-Patterns to Avoid

### ❌ Anti-Pattern 1: Event-Based State Propagation

```csharp
// BAD: Using events instead of observables
public class GameService
{
    public event Action? OnGameStateChanged;

    public void StartNewGame()
    {
        game = new Game(...);
        OnGameStateChanged?.Invoke(); // Manual notification
    }
}

// Component subscribes to event
GameService.OnGameStateChanged += () => StateHasChanged();
```

**Why bad**: Defeats the purpose of Rx, loses composability, harder to manage lifecycle

### ❌ Anti-Pattern 2: Polling/Manual Refresh

```csharp
// BAD: Manually refreshing state
private void RefreshGameState()
{
    currentPhase = GameService.CurrentGame?.Phase ?? GamePhase.Setup;
    isReady = GameService.CurrentGame?.Arena.Ready ?? false;
    StateHasChanged();
}
```

**Why bad**: Not reactive, requires manual calls, prone to stale data

### ❌ Anti-Pattern 3: Mixed Observable and Event Patterns

```csharp
// BAD: Inconsistent mix of observables and events
game.ObservablePhase.Subscribe(...);
GameService.OnGridChanged += ...;
```

**Why bad**: Inconsistent, confusing, harder to maintain

### ❌ Anti-Pattern 4: Forgetting to Dispose Subscriptions

```csharp
// BAD: Memory leak
protected override void OnInitialized()
{
    game.ObservablePhase.Subscribe(phase => currentPhase = phase);
    // No disposal! Subscription lives forever
}
```

**Why bad**: Memory leaks, zombie subscriptions continue executing

### ❌ Anti-Pattern 5: Blocking Observables

```csharp
// BAD: Blocking UI thread
var phase = game.ObservablePhase.First(); // Blocks until first emission
```

**Why bad**: Defeats async nature, freezes UI

## Testing Reactive Components

### Testing Pattern 1: Observable Injection

```csharp
[Fact]
public void Component_ReactsToPhaseChanges()
{
    // Arrange
    var phaseSubject = new BehaviorSubject<GamePhase>(GamePhase.Setup);
    var mockGame = new Mock<IGame>();
    mockGame.Setup(g => g.ObservablePhase).Returns(phaseSubject);

    using var ctx = new TestContext();
    ctx.Services.AddSingleton(mockGame.Object);

    // Act
    var component = ctx.RenderComponent<GameComponent>();
    phaseSubject.OnNext(GamePhase.Action);

    // Assert
    component.WaitForAssertion(() =>
        component.Find(".phase-indicator").TextContent.Should().Contain("Action")
    );
}
```

### Testing Pattern 2: Time-Based Testing

```csharp
[Fact]
public void Component_ThrottlesUpdates()
{
    // Use TestScheduler for testing time-based operators
    var scheduler = new TestScheduler();
    var observable = Observable.Interval(TimeSpan.FromSeconds(1), scheduler)
        .Take(5);

    var results = new List<long>();
    observable.Subscribe(results.Add);

    scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
    results.Should().HaveCount(3);
}
```

## Migration Strategy

To migrate existing components from event-based to observable-based:

1. **Identify State Sources**: Find all `event` declarations in services
2. **Trace to Observables**: Identify the underlying engine observables
3. **Update Service**: Remove events, expose observables or engine entities
4. **Update Components**:
   - Replace event subscriptions with observable subscriptions
   - Add `ObserveOn(SynchronizationContext.Current!)`
   - Add subscriptions to disposal list
   - Remove manual `StateHasChanged()` calls where observables handle it
5. **Test**: Verify state updates flow correctly
6. **Cleanup**: Remove unused event handlers

### Example Migration

**Before**:
```csharp
// Service
public event Action? OnGameStateChanged;
game.ObservablePhase.Subscribe(_ => OnGameStateChanged?.Invoke());

// Component
GameService.OnGameStateChanged += OnGameStateChanged;
private void OnGameStateChanged() => InvokeAsync(() => { LoadGrids(); StateHasChanged(); });
```

**After**:
```csharp
// Service
public Game? CurrentGame => game; // Just expose the game

// Component
var game = GameService.CurrentGame;
game.ObservablePhase
    .ObserveOn(SynchronizationContext.Current!)
    .Subscribe(phase => { currentPhase = phase; StateHasChanged(); });
```

## Summary Checklist

When implementing a new Blazor component with reactive patterns:

- [ ] Component implements `IDisposable`
- [ ] Component maintains `List<IDisposable> subscriptions`
- [ ] Subscriptions created in `OnInitialized` or `OnParametersSet`
- [ ] All subscriptions use `ObserveOn(SynchronizationContext.Current!)`
- [ ] All subscriptions added to `subscriptions` list
- [ ] `Dispose()` method disposes all subscriptions
- [ ] No manual event handlers for state propagation
- [ ] State updates flow through observables, not method calls
- [ ] Unit tests verify observable behavior
- [ ] No calls to `StateHasChanged()` outside of observable subscription handlers

---

**Version**: 1.0
**Last Updated**: 2025-11-28
**Status**: Active
