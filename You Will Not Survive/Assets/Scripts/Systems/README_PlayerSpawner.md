# Player Spawner System

This system allows you to spawn player entities using DOTS ECS with either SubScene references or direct prefab spawning.

## Setup Instructions

### Method 1: Using SubScene (Recommended for complex player setups)

1. **Create a Player SubScene:**
   - Create a new scene for your player
   - Add a GameObject with `PlayerAuthoring` component
   - Add visual representation (SpriteRenderer, etc.)
   - Save the scene
   - Add the scene as a SubScene to your main scene

2. **Set up the PlayerSpawner:**
   - Create a GameObject in your main scene
   - Add the `PlayerSpawnerAuthoring` component
   - Assign the player SubScene to the `playerSubScene` field
   - Set `usePrefabSpawn` to `false`
   - Set the desired `spawnPosition`

### Method 2: Using Direct Prefab (Simpler approach)

1. **Create a Player Prefab:**
   - Create a GameObject with `PlayerPrefabAuthoring` component
   - Add visual representation (SpriteRenderer, etc.)
   - Configure the player settings in the inspector
   - Create a prefab from this GameObject

2. **Set up the PlayerSpawner:**
   - Create a GameObject in your main scene
   - Add the `PlayerSpawnerAuthoring` component
   - Assign the player prefab to the `playerPrefab` field
   - Set `usePrefabSpawn` to `true`
   - Set the desired `spawnPosition`

## How It Works

The `PlayerSpawnerSystem` runs every frame and:

1. **For SubScene spawning:**
   - Loads the SubScene asynchronously
   - Waits for the scene to finish loading
   - Finds the player entity in the loaded scene
   - Sets its position to the spawn position

2. **For Prefab spawning:**
   - Instantiates the player prefab directly
   - Sets its position to the spawn position

## Components Used

- `PlayerSpawnerComponent`: Holds spawner configuration
- `PlayerTagComponent`: Identifies player entities
- `PlayerMovementComponent`: Player movement data
- `PlayerInputComponent`: Player input data
- `Player2DComponent`: 2D-specific player data

## Troubleshooting

- **No player spawned:** Check that the SubScene/prefab is assigned and contains a player entity
- **Player not visible:** Ensure the player has proper visual components (SpriteRenderer, etc.)
- **Components not visible in runtime:** Make sure the player entity has the `PlayerTagComponent` and other required components

## Example Usage

```csharp
// The system automatically handles spawning when:
// 1. PlayerSpawnerComponent exists in the world
// 2. ShouldSpawn is true
// 3. Either SubScene is loaded or prefab is assigned
```

The system will log messages to the console indicating the spawning progress and any issues encountered.


