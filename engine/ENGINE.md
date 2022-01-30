# Domain Entities

- Player
- Board
- Game
- Square
- Vessel
- VesselSelection
- Phase

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
  - ArenaCreated (begin phase 1)
  - BattleCommensed (begin phase 2)
- ActionAvailable
- SquareHit
- VesselSunk
- Victory