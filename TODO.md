# Outstanding Features

## Tier 0: Fix Observable Pattern Implementation

The engine uses System.Reactive's `BehaviorSubject<T>` to implement state change notifications across Game, Arena, and Grid classes. The current implementation has critical issues including memory leaks from unmanaged subscriptions, lack of resource cleanup, missing error handling, and thread safety concerns.

**Priority Action Items:**

*   [x] **CRITICAL: Fix Memory Leak** - Store subscription IDisposable in Game.cs:48-56 instead of discarding it
*   [x] **HIGH: Implement IDisposable** - Add IDisposable interface to Game, Arena, and Grid classes with proper cleanup (completed for Game class)
*   [ ] **MEDIUM: Add Error Handling** - Implement onError handlers for all observable subscriptions
*   [ ] **MEDIUM: Fix Test Assertion** - Add missing assertion in ArenaTests.cs:27 to verify readyObserved
*   [ ] **LOW: Document Observable Lifecycle** - Document when subjects should call OnCompleted()
*   [ ] **CONSIDER: Evaluate Pattern Choice** - Assess if System.Reactive adds enough value vs. simpler C# events

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
