# Outstanding Features

## Tier 0: Test Coverage Improvements

Code coverage analysis revealed critical gaps in testing (74.71% line coverage, 44.04% branch coverage). Priority areas for improvement:

**Priority Action Items:**

*   [ ] **CRITICAL: Add Serialization Tests** - GameSerializationExtensions.cs has 0% coverage (ToJson, FromJson, ToJsonFile, FromJsonFile)
*   [ ] **HIGH: Add Disposal Tests** - All Dispose() methods untested (Game, Arena, Grid) - memory leak risk
*   [ ] **MEDIUM: Test Error Paths** - Grid overlap validation (Grid.cs:48-50), out-of-bounds placement, invalid abandon attempts
*   [ ] **MEDIUM: Improve Branch Coverage** - Test Coords equality operators (==, !=), GridFullException.Message property
*   [ ] **LOW: Add ManualGrid Tests** - ManualGrid.cs has 0% coverage

## Tier 1: Game Engine - Action Phase Implementation
*   [ ] **Attack Logic**: Implement the ability for a player to target a coordinate on the opponent's grid.
*   [ ] **Hit/Miss Detection**: Determine if an attack hits a vessel or water.
*   [ ] **Vessel Sinking**: Logic to track damage to vessels and determine when a vessel is completely sunk.
*   [ ] **Turn Management**: Enforce turn-based gameplay (Player 1 -> Player 2 -> Player 1).
*   [ ] **Victory Conditions**: Check after every move if a player has lost all vessels.
*   [ ] **Game End State**: Transition game phase to `Ended` and declare a winner.
*   [ ] **Command/Event System**: Formalize the communication pattern between UI and Engine (as mentioned in ARCHITECTURE.md).

## Tier 2: User Interface & Experience
*   [ ] **Playable Interface**: Create a Blazor WebAssembly SPA game client.
*   [ ] **Project Setup**: Initialize a new Blazor WASM project and reference the Engine.
*   [ ] **Game Loop Integration**: Connect the interface to the engine's state changes.
*   [ ] **Visual Feedback**: Display hits, misses, and sunk vessels to the player.
*   [ ] **Input Handling**: Allow players to input coordinates for attacks.
*   [ ] **Game Setup UI**: Allow players to place vessels (Manual or Auto).
