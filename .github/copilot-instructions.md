The project is called "Conflict Vessels," a turn-based game about seafaring vessels in armed conflict.

The game engine is implemented in C# (.NET 6), located in `engine/ConflictVessels.Engine/`.

The user interface is separate and currently includes a CLI tool in `interfaces/Checker/`.

The engine uses domain entities: Game, Arena, Board, Player, Vessel, VesselSelection.

Commands and events are used for communication between the engine and UI.

The engine supports commands for creating/abandoning games, placing vessels, and attacking squares.

Events include game phases, victory, grid bombardment, vessel sunk, and player actions.

Tests for the engine are in `engine/ConflictVessels.Engine.Tests/`, organized by domain (Arena, Game, Player, Grids, Vessels).

Vessel placement and grid logic are in `engine/ConflictVessels.Engine/Grids/`.

Vessel logic is in `engine/ConflictVessels.Engine/Vessels/`.

The CLI tool in `interfaces/Checker/` can generate and print random battle grids.

Project documentation is in the root and engine folders (`README.md`, `ENGINE.md`, `RULES.md`, `ARCHITECTURE.md`).

Requirements and feature requests are in the `requirements/` folder.

Use .NET CLI for building, testing, and running the solution.
