# Gameplay

This document describes the gameplay experience of Conflict Vessels from a player's perspective, including core mechanics, game flow, and strategic elements.

## Overview

Conflict Vessels is a turn-based naval warfare game where two or more players compete to locate and destroy their opponent's fleet. The game combines strategic placement with tactical decision-making in a battle of wits on the high seas.

## Game Objective

**Sink all enemy vessels before they sink yours.**

Victory is achieved by successfully attacking every square occupied by an opponent's fleet, causing all their vessels to be revealed as sunk. The last player with vessels remaining wins the battle.

## Core Mechanics

### The Battlefield

Each player has their own private grid - a square or rectangular battleground divided into spaces:

- **Coordinates**: Each space has an X/Y coordinate (X is numeric, Y is alphabetic)
- **Size**: Typically 10x10, but customizable
- **Vessels**: Spaces can contain parts of vessels or be empty water

### The Fleet

Players command a fleet of vessels of varying sizes:

**Standard Fleet:**
- 1x Carrier (5 spaces)
- 1x Battleship (4 spaces)
- 2x Cruisers (3 spaces each)
- 1x Destroyer (2 spaces)

Each vessel occupies consecutive spaces in either horizontal or vertical orientation. Vessels cannot overlap with each other.

## Game Flow

Conflict Vessels progresses through distinct phases:

### Phase 1: Setup

**Objective**: Position your fleet strategically on your grid.

**Actions:**
1. You receive your fleet of vessels
2. Place each vessel on your grid by selecting:
   - **Position**: Starting coordinate (e.g., A5, C2)
   - **Orientation**: Horizontal or Vertical
3. Ensure each vessel:
   - Fits completely within the grid boundaries
   - Does not overlap with previously placed vessels

**Strategic Considerations:**
- Spread vessels to avoid clustered targeting
- Use edges and corners to reduce attack vectors
- Consider common attack patterns when placing
- Balance between hiding and avoiding predictable placement

**Alternative**: Some game modes may use **AutoGrid**, which randomly places vessels for quick play.

### Phase 2: Action (Battle Phase)

**Objective**: Locate and destroy all enemy vessels before they destroy yours.

**Turn Structure:**

Each turn, the active player:

1. **Nominates a Target**
   - Select one coordinate on the opponent's grid to attack
   - Cannot select a coordinate that has already been attacked this game

2. **Receives Attack Result**
   - **Miss**: The attack hit empty water
   - **Hit**: The attack struck a vessel
   - **Sunk**: The attack was the final hit on a vessel
     - You are informed which type of vessel was destroyed

3. **Turn Passes**
   - Next player takes their turn

**Battle Information:**
- You see which coordinates you've attacked previously
- You know which attacks were hits vs. misses
- You learn the type of vessel when you sink it

**Strategic Elements:**
- **Pattern Recognition**: Identify vessel locations from hit patterns
- **Deduction**: Determine vessel orientation and size
- **Probability**: Target high-probability areas first
- **Systematic Searching**: Cover the grid efficiently
- **Follow-up Attacks**: After a hit, attack adjacent spaces to find the rest of the vessel

### Phase 3: Game End

The battle concludes when:

- **Victory**: One player sinks all enemy vessels
- **Defeat**: All your vessels are sunk
- **Abandonment**: A player withdraws from the game

## Victory Conditions

**You win when:**
- All vessels on every opponent's grid have been sunk
- You are the last player with vessels remaining

**You lose when:**
- All your vessels have been sunk
- You abandon the game

## Strategic Depth

### Setup Strategy

**Placement Patterns:**
- **Scattered**: Vessels spread across the grid (harder to find)
- **Edge Hugging**: Vessels along borders (fewer attack angles)
- **Clustered**: Vessels grouped (risky but unpredictable)
- **Corners**: Using corner positions for protection

**Considerations:**
- Most players attack the center first
- Edge placement reduces possible attack directions
- Diagonal spacing can confuse systematic searchers

### Battle Strategy

**Search Patterns:**
- **Checkerboard**: Attack every other square to find vessels efficiently
- **Diagonal Lines**: Cover maximum area with diagonal sweeps
- **Spiral**: Work from outside in or center out
- **Random**: Unpredictable but less efficient

**After Scoring a Hit:**
1. Attack adjacent squares (up, down, left, right)
2. Determine orientation when you get a second hit
3. Continue attacking in that direction until vessel is sunk
4. Remember sunk vessel size to narrow down remaining fleet

**Advanced Tactics:**
- Track which vessel sizes remain unsunk
- Calculate probability based on remaining spaces
- Focus on areas large enough to contain remaining vessels
- Use opponent attack patterns to deduce their strategy

## Game Variations

### Number of Players

While standard Conflict Vessels is a 2-player game, the architecture supports:

- **Two-Player**: Classic head-to-head battle
- **Multiplayer**: Free-for-all or team-based combat (future)

### Grid Sizes

Different grid dimensions change the game dynamics:

- **Small (7x7)**: Fast, aggressive gameplay
- **Standard (10x10)**: Balanced difficulty
- **Large (12x12+)**: Extended battles, more hiding space

### Fleet Composition

Custom vessel sets can alter difficulty and strategy:

- **Classic**: Standard 5-vessel fleet
- **Armada**: Larger fleet with more vessels
- **Minimal**: Fewer vessels for quick games
- **Mixed**: Unusual vessel size combinations

## Example Turn Sequence

**Turn 1 - Player 1:**
- Attacks coordinate D5
- Result: Miss
- Player 2's turn

**Turn 2 - Player 2:**
- Attacks coordinate E4
- Result: Hit!
- Player 1's turn

**Turn 3 - Player 1:**
- Attacks coordinate B7
- Result: Hit!
- Player 2's turn

**Turn 4 - Player 2:**
- Attacks coordinate E5 (adjacent to previous hit)
- Result: Hit!
- Player 1's turn

**Turn 5 - Player 1:**
- Attacks coordinate B8 (adjacent to previous hit)
- Result: Sunk! - Destroyer (size 2)
- Player 1 now knows the entire Destroyer is eliminated
- Player 2's turn

...and so on until all of one player's vessels are destroyed.

## Learning Curve

### Beginner-Friendly
- Simple core concept: hide your ships, find theirs
- Easy to understand rules
- No complex mechanics to master

### Skill Development
- **Early Games**: Random placement and attacking
- **Intermediate**: Pattern recognition and systematic searching
- **Advanced**: Probability calculation and psychological gameplay

### Mastery Elements
- Optimal vessel placement strategy
- Efficient search patterns
- Adaptive tactics based on opponent behavior
- Statistical analysis of hit/miss ratios

## Tips for New Players

1. **Don't cluster vessels** - One good hit shouldn't reveal multiple ships
2. **Attack systematically** - Random attacks are inefficient
3. **Use edges strategically** - Border placement reduces attack angles
4. **Follow up hits immediately** - Don't move to a new area until you've sunk the vessel
5. **Track your attacks** - Remember where you've already struck
6. **Learn from each game** - Analyze what placement and attack strategies work

## Current Implementation Status

### Implemented
- Setup Phase with vessel placement
- Grid management and validation
- AutoGrid for automatic vessel placement
- Game state tracking
- Save/Load functionality

### In Development
- Action Phase mechanics (attacking)
- Hit/Miss detection
- Vessel sinking logic
- Turn management
- User interface

For technical implementation details, see [ARCHITECTURE.md](ARCHITECTURE.md).

For formal game rules, see [RULES.md](RULES.md).
