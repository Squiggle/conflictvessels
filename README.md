# Conflict Vessels

A turn-based game about seafaring vessels in a situation of armed conflict.

## Rules

For a programatical breakdown of the rules, see [RULES.md](RULES.md).

## Architecture

The implementation of this game is comprised of the Game Engine and the User
Interface.

- The Game Engine is implemented in C#.
- The User Interface is implemented in [TBC].

Communication between the Game Engine and User Interface is performed through
Commands and Events.

# User Interface

Currently the only interface is a command line tool to visualise the generated battle grids.

See /interfaces/Checker for a dotnet CLI which can generate randomly placed vessels for grids of any size, with any number of differently sized vessels.

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
