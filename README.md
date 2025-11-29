# Conflict Vessels

A turn-based game about seafaring vessels in a situation of armed conflict.

## Rules

For a programatical breakdown of the rules, see [RULES.md](RULES.md).

## Architecture

The implementation of this game is comprised of the Game Engine and the User
Interface.

- The Game Engine is implemented in C#.
- The User Interface is implemented using Blazor WebAssembly (web) and CLI tools.

Communication between the Game Engine and User Interface is performed through
Commands and Events.

# User Interfaces

## Web Interface (Blazor WebAssembly)

A browser-based interactive interface with CSS animations for gameplay visualization.

To run the web interface:

```bash
cd interfaces/ConflictVessels.Web
dotnet run
```

Then navigate to `http://localhost:5000` in your browser and click "Play Game".

Features:
- Interactive grid visualization
- CSS animations for smooth effects
- Reactive state management
- Responsive design
- Runs entirely in the browser (no server required)

See [interfaces/ConflictVessels.Web/README.md](interfaces/ConflictVessels.Web/README.md) for more details.

## Command Line Interface (Checker)

A CLI tool to visualize randomly generated battle grids.

See [interfaces/Checker/README.md](interfaces/Checker/README.md) for usage details.

```csharp
var vessels = new Vessel[] {
  new Vessel(2),
  new Vessel(3),
  new Vessel(3),
  new Vessel(4),
  new Vessel(5)
};

var game = new Game(
  new Arena(new AutoGrid(10, 10, vessels))
);

foreach (var grid in game.Arena.Grids)
{
  grid.Print();
}
```
