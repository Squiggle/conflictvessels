# Domain Entities

- Game - manages the players and phases of the game
- Arena - manages the game boards
- Board - defines the grid, owned by a player
- Player - manages identity and access to a Grid within the Arena
- Vessel - has a size and description, can be placed upon a Grid, can be struck
  with a bombardment
- VesselSelection - a pool of vessels of varying sizes

# Commands

- Game
  - Create
  - Abandon
- Vessel
  - Place
- Board
  - PlaceVessel
- Square
  - Attack

# Events

- Game
  - Phases
  - Victory
- Grid
  - Bombarded
  - VesselSunk
- Player
  - ActionAvailable
