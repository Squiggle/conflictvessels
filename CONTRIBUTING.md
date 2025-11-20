# Contributing to Conflict Vessels

This guide outlines the coding standards, patterns, and practices used in the Conflict Vessels codebase.

## Table of Contents

- [Language and Frameworks](#language-and-frameworks)
- [Code Organization](#code-organization)
- [Naming Conventions](#naming-conventions)
- [Coding Style](#coding-style)
- [Design Patterns](#design-patterns)
- [Testing](#testing)
- [Documentation](#documentation)
- [Git Workflow](#git-workflow)

## Language and Frameworks

### Primary Stack

- **Language**: C# (modern .NET)
- **Test Framework**: xUnit
- **Reactive Extensions**: System.Reactive (Rx.NET)
- **Serialization**: System.Text.Json

### Project Structure

```
conflictvessels/
├── engine/
│   ├── ConflictVessels.Engine/        # Core game engine
│   └── ConflictVessels.Engine.Tests/  # Engine tests
├── interfaces/
│   └── Checker/                        # CLI visualization tool
└── docs/                               # Documentation
```

## Code Organization

### Namespace Structure

Follow the folder structure for namespaces:

```csharp
// File: engine/ConflictVessels.Engine/Game.cs
namespace ConflictVessels.Engine;

// File: engine/ConflictVessels.Engine/Grids/Grid.cs
namespace ConflictVessels.Engine.Grids;

// File: engine/ConflictVessels.Engine/Vessels/Vessel.cs
namespace ConflictVessels.Engine.Vessels;
```

### File-Scoped Namespaces

Use modern C# file-scoped namespace declarations (single semicolon):

```csharp
namespace ConflictVessels.Engine;

public class Game
{
  // class implementation
}
```

**Not:**
```csharp
namespace ConflictVessels.Engine
{
  public class Game
  {
    // class implementation
  }
}
```

### One Entity Per File

Each class, enum, or interface should have its own file with a matching name:

- `Game.cs` contains `class Game`
- `GamePhase.cs` contains `enum GamePhase`
- `VesselOrientation.cs` contains `enum VesselOrientation`

**Exception**: Small, tightly coupled helper classes (e.g., `GridFullException` in `AutoGrid.cs:39`)

## Naming Conventions

### Classes and Types

- **PascalCase** for classes, enums, interfaces, structs
- Examples: `Game`, `Arena`, `VesselGridPlacement`, `GamePhase`

### Properties and Methods

- **PascalCase** for public properties and methods
- Examples: `Width`, `Height`, `Place()`, `Default()`

### Fields

- **camelCase** for private fields
- Prefix with `readonly` when applicable
- Examples: `players`, `readySubject`, `vessels`

### Method Parameters

- **camelCase** for parameters
- Use descriptive names: `vessel`, `orientation`, `filePath`

### Constants

- **PascalCase** for constants
- Example: `Options` (in GameSerializationExtensions.cs:14)

### Enums

- **PascalCase** for enum names and values
- Example:
  ```csharp
  public enum GamePhase
  {
    Setup,
    Action,
    Ended
  }
  ```

## Coding Style

### Properties

Use init-only properties for immutable data:

```csharp
public Guid Id { get; init; }
public int Width { get; init; }
public Arena Arena { get; init; }
```

Use private setters for internally managed state:

```csharp
public GamePhase Phase
{
  get => phaseSubject.Value;
  private set => phaseSubject.OnNext(value);
}
```

### Collections

Expose collections as read-only to prevent external modification:

```csharp
private readonly List<Player> players;
public ReadOnlyCollection<Player> Players => players.AsReadOnly();
```

For observable collections that notify subscribers, use `ObservableCollection<T>`:

```csharp
public ObservableCollection<string> PlayerActions { get; init; }
```

### Constructor Initialization

Initialize collections in constructors, not as field initializers when dependencies are involved:

```csharp
public Game(Arena arena, params Player[] players)
{
  Id = Guid.NewGuid();
  this.players = players.ToList();
  Arena = arena;
  PlayerActions = new ObservableCollection<string>();
}
```

### Factory Methods

Provide static `Default()` factory methods for common configurations:

```csharp
public static Game Default()
{
  return new Game(Arena.Default(), new Player(), new Player());
}
```

### Using Declarations

Place `using` statements at the top of files, outside the namespace (modern C# style):

```csharp
using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

namespace ConflictVessels.Engine;

public class Game { }
```

### Expression-Bodied Members

Use for simple property getters and single-line methods:

```csharp
public bool Active => phaseSubject.Value != GamePhase.Ended;
```

### Switch Expressions

Prefer modern switch expressions over switch statements:

```csharp
var availableGrid = orientation switch
{
  VesselOrientation.Horizontal => new Grid(Width - (vessel.Vessel.Size - 1), Height),
  VesselOrientation.Vertical => new Grid(Width, Height - (vessel.Vessel.Size - 1)),
  _ => throw new ArgumentException(nameof(orientation))
};
```

### LINQ and Functional Style

Embrace LINQ for collection operations:

```csharp
var options = availableGrid.Coords()
  .Except(occupiedCoords)
  .ToList();

IEnumerable<Coords> occupiedCoords = Vessels
  .SelectMany(v => v.Coords)
  .SelectMany(c => c.Shadow(vessel.Vessel.Size, orientation));
```

## Design Patterns

### Reactive Programming

Use **System.Reactive** for state change notifications:

```csharp
private readonly BehaviorSubject<GamePhase> phaseSubject =
  new BehaviorSubject<GamePhase>(GamePhase.Setup);

public IObservable<GamePhase> ObservablePhase => phaseSubject;

// Subscribe to changes
arena.ObservableReady.Subscribe(ready => {
  if (Active) {
    Phase = ready ? GamePhase.Action : GamePhase.Setup;
  }
});
```

**Pattern Usage:**
- Expose `IObservable<T>` for properties that change over time
- Use `BehaviorSubject<T>` internally to store current value and notify subscribers
- Subscribe in constructors to establish reactive relationships

### Factory Pattern

Provide default factories for common use cases:

```csharp
public static Game Default()
public static Arena Default()
public static Grid Default()
```

### Strategy Pattern

Use inheritance for different strategies of the same concept:

```csharp
public class Grid { }
public class AutoGrid : Grid { }  // Automatic placement
public class ManualGrid : Grid { } // Future: manual placement
```

### Encapsulation

Keep fields private, expose through properties:

```csharp
private readonly List<VesselGridPlacement> vessels;
public ReadOnlyCollection<VesselGridPlacement> Vessels => vessels.AsReadOnly();
```

### Extension Methods

Group related extension methods in static classes:

```csharp
public static class GameSerializationExtensions
{
  public static string ToJson(this Game game) { }
  public static void ToJsonFile(this Game game, string filePath) { }
  public static Game FromJson(string json) { }
  public static Game FromJsonFile(string filePath) { }
}
```

## Testing

### Framework and Structure

- Use **xUnit** for all tests
- Organize tests in a separate project: `ConflictVessels.Engine.Tests`
- Mirror the source folder structure in test project

### Test Naming

Use descriptive, sentence-style names with underscores:

```csharp
[Fact]
public void Create_game_initialises_new_game_with_players()

[Fact]
public void Game_can_be_abandoned_by_player()

[Fact]
public void Game_commenses_battle_when_all_vessels_have_been_placed()
```

**Pattern**: `Action_result_conditions` or `Subject_action_outcome`

### Test Structure (AAA Pattern)

Organize tests using Arrange-Act-Assert:

```csharp
[Fact]
public void Game_commenses_battle_when_all_vessels_have_been_placed()
{
  // Arrange
  var testArena = new TestArena();
  var game = new Game(testArena, new Player());

  GamePhase? phaseObserved = null;
  game.ObservablePhase.Subscribe(phase => phaseObserved = phase);

  // Act
  testArena.ReadyUp();

  // Assert
  Assert.Equal(GamePhase.Action, game.Phase);
  Assert.Equal(GamePhase.Action, phaseObserved);
  Assert.True(game.Active);
}
```

### Test Helpers

Create internal helper classes within test files for controlled scenarios:

```csharp
internal class TestArena : Arena
{
  public void ReadyUp() => Ready = true;
  public void NotReady() => Ready = false;
}
```

### Assertions

Use xUnit's `Assert` class:

```csharp
Assert.Equal(expected, actual);
Assert.True(condition);
Assert.False(condition);
Assert.NotNull(obj);
Assert.NotEqual(Guid.Empty, game.Id);
Assert.Empty(collection);
```

## Documentation

### XML Documentation Comments

Provide XML comments for public classes and methods:

```csharp
/// <summary>
/// Serializes the Game instance to a JSON string.
/// </summary>
public static string ToJson(this Game game)
{
  if (game == null) throw new ArgumentNullException(nameof(game));
  return JsonSerializer.Serialize(game, Options);
}
```

Use `<summary>` tags consistently:

```csharp
/// <summary>Identifier</summary>
public Guid Id { get; init; }

/// <summary>Arena for the Game</summary>
public Arena Arena { get; init; }
```

### Inline Comments

Use inline comments sparingly, only when code intent is not obvious:

```csharp
// toggle Phase based on the readiness of the Arena
arena.ObservableReady.Subscribe(ready => { ... });

// vertical or horizontal?
var orientation = new Random().Next(0, 2) == 0
  ? VesselOrientation.Horizontal
  : VesselOrientation.Vertical;
```

### README and Architecture Docs

Maintain high-level documentation:

- **README.md**: Project overview, quickstart, basic usage
- **RULES.md**: Game rules and mechanics
- **ARCHITECTURE.md**: Technical design, entities, patterns
- **CONTRIBUTING.md**: This document
- **docs/**: Detailed technical documentation (schemas, APIs)

## Git Workflow

### Branch Structure

- **master**: Main development branch (currently used)
- Feature branches: Create for significant features (recommended for future)

### Commit Messages

Write clear, imperative commit messages:

```
Add save/load functionality with JSON serialization

- Implement GameSerializationExtensions
- Add ToJson/FromJson methods
- Create GameSaveSchema.md documentation
```

**Format:**
- First line: Imperative summary (50 chars or less)
- Blank line
- Detailed bullet points explaining changes

### Recent Commit Examples

```
2f070ec Vibe coding - adding a save/load function including JSON file format
99d2fe1 Update README.md
3bb82e8 Created a CLI to visualise grids
```

### Commit Hygiene

- Commit logical units of work
- Don't commit commented-out code
- Don't commit build artifacts or IDE-specific files
- Update documentation in the same commit as code changes when applicable

### Pull Requests (Future)

When the project adopts PRs:

1. Create descriptive PR titles
2. Reference related issues
3. Include testing notes
4. Request review from maintainers
5. Squash commits if needed before merging

## Code Review Checklist

When reviewing code, check for:

- [ ] Follows naming conventions
- [ ] Properties use appropriate access modifiers (init, private set)
- [ ] Collections exposed as ReadOnly
- [ ] XML documentation on public members
- [ ] Tests follow AAA pattern
- [ ] Reactive subscriptions properly established
- [ ] Exceptions have meaningful messages
- [ ] LINQ used appropriately
- [ ] No hardcoded magic numbers (use constants or properties)
- [ ] Switch expressions include exhaustive cases

## Additional Best Practices

### Immutability

Prefer immutable data structures where possible:

```csharp
public Guid Id { get; init; }  // Can only be set during initialization
```

### Null Handling

Use null checks and throw descriptive exceptions:

```csharp
if (game == null) throw new ArgumentNullException(nameof(game));
if (string.IsNullOrWhiteSpace(filePath))
  throw new ArgumentException("File path must not be null or empty.", nameof(filePath));
```

### Exception Messages

Provide context in exception messages:

```csharp
throw new ArgumentException("Given Player is not participating in this game");

public override string Message =>
  $"Vessel of size {VesselThatCannotFit.Size} cannot fit in a {Orientation} position on this grid";
```

### Avoid Magic Numbers

Use meaningful variable names instead of literals:

```csharp
// Good
var standardVessels = new Vessel[] {
  new Vessel(2),
  new Vessel(3),
  new Vessel(3),
  new Vessel(4),
  new Vessel(5)
};

// Could be improved with constants
const int DESTROYER_SIZE = 2;
const int SUBMARINE_SIZE = 3;
// etc.
```

## Questions or Suggestions?

If you have questions about these guidelines or suggestions for improvements, please open an issue or discussion in the repository.
