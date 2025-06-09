# Conflict Vessels Game Save Schema

This document describes the JSON schema used for serializing and deserializing a game state in Conflict Vessels. The schema ensures that all required information for restoring a game is present and correctly structured.

## Top-Level Structure

A valid game save JSON object must contain the following properties:

- `id` (string, uuid): Unique identifier for the game.
- `phase` (string): Current phase of the game. One of `Setup`, `Action`, or `Ended`.
- `arena` (object): The arena, containing player grids.
- `players` (array): List of player objects (minimum 2).
- `playerActions` (array, optional): History of player actions (strings).

## Schema Details

### Game Object
| Property        | Type    | Description                                 |
|----------------|---------|---------------------------------------------|
| id             | string  | Unique identifier (UUID) for the game.      |
| phase          | string  | Game phase: `Setup`, `Action`, or `Ended`.  |
| arena          | object  | Arena object (see below).                   |
| players        | array   | List of player objects (see below).         |
| playerActions  | array   | (Optional) List of player action strings.   |

### Arena Object
| Property | Type  | Description                |
|----------|-------|----------------------------|
| grids    | array | Array of grid objects.     |

### Player Object
| Property | Type   | Description                      |
|----------|--------|----------------------------------|
| id       | string | Unique identifier (UUID) for player. |

### Grid Object
| Property | Type    | Description                        |
|----------|---------|------------------------------------|
| width    | integer | Width of the grid (>=1).           |
| height   | integer | Height of the grid (>=1).          |
| vessels  | array   | Array of vesselGridPlacement objects. |

### VesselGridPlacement Object
| Property | Type   | Description                        |
|----------|--------|------------------------------------|
| vessel   | object | Vessel object (see below).         |
| position | object | Vessel position object (see below).|

### Vessel Object
| Property | Type    | Description                |
|----------|---------|----------------------------|
| size     | integer | Size of the vessel (>=1).  |

### VesselPosition Object
| Property   | Type   | Description                        |
|------------|--------|------------------------------------|
| coords     | object | Coordinates object (see below).    |
| orientation| string | `Vertical` or `Horizontal`.        |

### Coords Object
| Property | Type    | Description                |
|----------|---------|----------------------------|
| x        | integer | X coordinate (>=0).        |
| y        | integer | Y coordinate (>=0).        |

## Example Game Save JSON

```json
{
  "id": "b1e7c2a0-1234-4f8a-9b2e-1a2b3c4d5e6f",
  "phase": "Action",
  "arena": {
    "grids": [
      {
        "width": 10,
        "height": 10,
        "vessels": [
          {
            "vessel": { "size": 4 },
            "position": {
              "coords": { "x": 2, "y": 3 },
              "orientation": "Horizontal"
            }
          }
        ]
      },
      {
        "width": 10,
        "height": 10,
        "vessels": []
      }
    ]
  },
  "players": [
    { "id": "d2f1e3c4-5678-4a9b-8c2d-3e4f5a6b7c8d" },
    { "id": "e3c4d2f1-6789-4b8a-9c2d-4f5a6b7c8d9e" }
  ],
  "playerActions": [
    "Player1 placed vessel at (2,3)",
    "Player2 attacked (2,3)"
  ]
}
```

This example demonstrates a minimal valid game state, including two players, an arena with two grids, and a vessel placed on one grid.
