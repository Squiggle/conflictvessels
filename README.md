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
