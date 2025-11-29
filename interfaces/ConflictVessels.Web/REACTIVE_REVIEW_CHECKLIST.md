# Reactive UI Code Review Checklist

Use this checklist when reviewing Blazor components to ensure they follow reactive patterns correctly.

## Service Layer Review

### Observable Exposure
- [ ] Services expose engine entities directly (e.g., `public Game? CurrentGame => game;`)
- [ ] Services expose engine observables directly when appropriate
- [ ] Services do NOT wrap observables in C# events (`event Action?`)
- [ ] Services do NOT have manual notification calls (`OnSomethingChanged?.Invoke()`)

### Service State Management
- [ ] Services manage their own subscriptions if they maintain internal state
- [ ] Services properly dispose all internal subscriptions
- [ ] Services implement `IDisposable` if they hold subscriptions

### Example Service (GOOD)
```csharp
public class GameService : IDisposable
{
    private Game? game;
    private readonly List<IDisposable> subscriptions = [];

    public Game? CurrentGame => game; // ✅ Direct exposure

    public void StartNewGame()
    {
        DisposeCurrentGame();
        game = CreateGame();
    }

    public void Dispose()
    {
        DisposeCurrentGame();
    }
}
```

## Component Layer Review

### Lifecycle Management
- [ ] Component implements `IDisposable`
- [ ] Component has `private readonly List<IDisposable> subscriptions = [];`
- [ ] Component creates subscriptions in `OnInitialized()` or `OnParametersSet()`
- [ ] Component disposes all subscriptions in `Dispose()` method
- [ ] `Dispose()` clears the subscriptions list after disposing

### Observable Subscriptions
- [ ] All observable subscriptions use `.ObserveOn(SynchronizationContext.Current!)`
- [ ] All subscriptions are added to the `subscriptions` list
- [ ] Subscriptions update local component state (private fields)
- [ ] Subscriptions call `StateHasChanged()` after updating state
- [ ] No subscriptions block or use `.First()`, `.Wait()`, etc.

### State Updates
- [ ] Component state is updated ONLY through observable subscriptions
- [ ] No manual `LoadData()`, `RefreshState()`, or similar methods
- [ ] No event handlers that call `StateHasChanged()` outside of subscriptions
- [ ] Command methods (e.g., button clicks) only call engine methods, not update state

### Threading
- [ ] All observable subscriptions include `ObserveOn(SynchronizationContext.Current!)`
- [ ] No direct access to observables without thread marshalling
- [ ] `StateHasChanged()` only called from UI thread (via `ObserveOn`)

### Null Safety
- [ ] Component handles null game/service instances gracefully
- [ ] Subscriptions check for null before subscribing
- [ ] UI has null guards (`@if (game != null)`)

## Anti-Pattern Detection

### ❌ Event-Based State (BAD)
```csharp
// Service
public event Action? OnGameStateChanged;

// Component
GameService.OnGameStateChanged += HandleStateChange;
private void HandleStateChange() => StateHasChanged();
```
**Action**: Replace with observable subscription

### ❌ Manual Refresh (BAD)
```csharp
private void LoadGrids()
{
    player1Grid = GameService.GetPlayerGrid(0);
    player2Grid = GameService.GetPlayerGrid(1);
    StateHasChanged();
}

// Called from event handler
GameService.OnGameStateChanged += LoadGrids;
```
**Action**: Subscribe to grid observable directly

### ❌ Missing ObserveOn (BAD)
```csharp
game.ObservablePhase.Subscribe(phase =>
{
    currentPhase = phase;
    StateHasChanged(); // May crash: wrong thread
});
```
**Action**: Add `.ObserveOn(SynchronizationContext.Current!)`

### ❌ Missing Disposal (BAD)
```csharp
protected override void OnInitialized()
{
    game.ObservablePhase.Subscribe(phase => currentPhase = phase);
    // Subscription never disposed! Memory leak!
}
```
**Action**: Add subscription to disposal list

### ❌ Mixed Patterns (BAD)
```csharp
// Some state via observables
game.ObservablePhase.Subscribe(...);

// Some state via events
GameService.OnGridChanged += ...;

// Some state via polling
LoadGrids();
```
**Action**: Use observables consistently for all state

## Example Component (GOOD)

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
        <div class="phase-indicator">Phase: @currentPhase</div>
        <GridComponent Grid="player1Grid" />
    }
</div>

@code {
    private readonly List<IDisposable> subscriptions = [];
    private GamePhase currentPhase = GamePhase.Setup;
    private Grid? player1Grid;

    protected override void OnInitialized()
    {
        SubscribeToGameState();
    }

    private void StartGame()
    {
        GameService.StartNewGame();
        SubscribeToGameState();
    }

    private void SubscribeToGameState()
    {
        // Clean up existing subscriptions
        foreach (var sub in subscriptions)
        {
            sub.Dispose();
        }
        subscriptions.Clear();

        var game = GameService.CurrentGame;
        if (game == null) return;

        // Subscribe to phase changes
        var phaseSub = game.ObservablePhase
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(phase =>
            {
                currentPhase = phase;
                StateHasChanged();
            });
        subscriptions.Add(phaseSub);

        // Load initial grid reference
        player1Grid = game.Arena.Grids[0];

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

## Review Questions

When reviewing a component, ask:

1. **Is state reactive?**
   - Does state flow through observables?
   - Or is it manually updated via method calls?

2. **Is threading correct?**
   - Are all subscriptions using `ObserveOn`?
   - Is `StateHasChanged()` only called on UI thread?

3. **Is cleanup correct?**
   - Are all subscriptions disposed?
   - Does `Dispose()` clear the subscriptions list?

4. **Is the pattern consistent?**
   - Is all state managed the same way?
   - No mixing of events, polling, and observables?

5. **Is it testable?**
   - Can observables be mocked/injected?
   - Can time-based behavior be tested with `TestScheduler`?

## Quick Fixes

### Fix 1: Convert Event to Observable

**Before**:
```csharp
// Service
public event Action? OnStateChanged;

// Component
GameService.OnStateChanged += () => StateHasChanged();
```

**After**:
```csharp
// Service
public Game? CurrentGame => game;

// Component
var sub = GameService.CurrentGame?.ObservablePhase
    .ObserveOn(SynchronizationContext.Current!)
    .Subscribe(_ => StateHasChanged());
if (sub != null) subscriptions.Add(sub);
```

### Fix 2: Add Disposal

**Before**:
```csharp
protected override void OnInitialized()
{
    game.ObservablePhase.Subscribe(phase => currentPhase = phase);
}
```

**After**:
```csharp
private readonly List<IDisposable> subscriptions = [];

protected override void OnInitialized()
{
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
```

### Fix 3: Add Thread Marshalling

**Before**:
```csharp
game.ObservablePhase.Subscribe(phase =>
{
    currentPhase = phase;
    StateHasChanged(); // May crash!
});
```

**After**:
```csharp
game.ObservablePhase
    .ObserveOn(SynchronizationContext.Current!)
    .Subscribe(phase =>
    {
        currentPhase = phase;
        StateHasChanged(); // Safe!
    });
```

## Migration Priority

When refactoring existing components:

1. **High Priority**: Fix missing `ObserveOn` (crashes)
2. **High Priority**: Fix missing disposal (memory leaks)
3. **Medium Priority**: Convert events to observables (architecture)
4. **Low Priority**: Optimize with Rx operators (performance)

## Testing Verification

After implementing reactive patterns, verify:

- [ ] Component renders correctly on initial load
- [ ] Component updates when observable emits new values
- [ ] No console errors about `StateHasChanged` on wrong thread
- [ ] Component cleans up properly when disposed
- [ ] No memory leaks (subscriptions disposed)
- [ ] bUnit tests pass with observable mocking

## Additional Resources

- See `REACTIVE_UI_PATTERNS.md` for detailed pattern documentation
- See `CONTRIBUTING.md` for general code standards
- See `ARCHITECTURE.md` for engine observable reference

---

**Version**: 1.0
**Last Updated**: 2025-11-28
