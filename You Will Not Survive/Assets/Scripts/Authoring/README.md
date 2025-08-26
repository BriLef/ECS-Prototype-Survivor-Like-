# Player Movement Setup Guide

This guide explains how to set up player movement using the ECS-based input and movement systems for 2D top-down games.

## Components Overview

### PlayerInputComponent
- Stores the current movement direction from input
- Automatically updated by PlayerInputSystem
- Uses both new Input System and legacy Input as fallback

### PlayerMovementComponent
- Contains movement settings (speed, rotation speed)
- Controls whether the player can move

### Player2DComponent
- **NEW**: 2D-specific functionality for top-down games
- Tracks movement state, facing direction, and movement threshold
- Provides better control over 2D movement behavior

### PlayerTagComponent
- Identifies entities as players
- Used by systems to find player entities

## Systems Overview

### PlayerInputSystem
- **UPDATED**: Now uses Unity's new Input System with fallback to legacy Input
- Reads configured Input Actions (WASD/Arrow keys)
- Updates PlayerInputComponent with normalized movement direction
- Automatically handles input system initialization

### PlayerMovementSystem
- **LEGACY**: Basic movement system (kept for compatibility)
- Applies movement to player entities based on input
- Uses the movement speed from PlayerMovementComponent

### Player2DMovementSystem
- **NEW**: Enhanced 2D movement system for top-down games
- Better movement state tracking
- Configurable movement threshold
- Improved rotation handling for 2D sprites
- **RECOMMENDED** for 2D top-down games

## How to Set Up a Player

### Option 1: Use the Menu (Recommended)
1. In the Unity Editor, go to `GameObject > 2D Object > Player Entity (2D Top-Down)`
2. This creates a complete player with:
   - PlayerAuthoring component
   - 2D sprite visual representation
   - Top-down orthographic camera
   - All necessary ECS components
3. Place the player in a SubScene for ECS conversion

### Option 2: Manual Setup
1. Create an empty GameObject
2. Add the `PlayerAuthoring` component
3. Configure movement settings in the inspector:
   - **Move Speed**: How fast the player moves (default: 5)
   - **Rotation Speed**: How fast the player rotates (default: 180)
   - **Move Threshold**: Minimum input to trigger movement (default: 0.1)
   - **Enable Rotation**: Whether to rotate towards movement direction
   - **Enable Input**: Whether input is active (default: true)
4. Add visual representation (2D sprite, SpriteRenderer component)
5. Place in a SubScene

## Input Controls

The player responds to:
- **W/Up Arrow**: Move up
- **S/Down Arrow**: Move down  
- **A/Left Arrow**: Move left
- **D/Right Arrow**: Move right

Movement is normalized, so diagonal movement isn't faster than cardinal directions.

## 2D Movement Features

### Movement Threshold
- Configurable minimum input threshold to prevent jittery movement
- Default value of 0.1 means very small stick movements won't move the player

### Facing Direction
- Player automatically faces the direction they're moving
- Smooth rotation prevents jarring direction changes
- Can be disabled if you prefer static sprites

### Movement State Tracking
- `IsMoving` boolean tracks current movement state
- `LastMoveDirection` stores the previous movement direction
- `FacingDirection` tracks which way the player is facing

## Customization

### Changing Movement Speed
- Adjust the `Move Speed` value in the PlayerAuthoring component
- Or modify the default value in PlayerMovementComponent

### Movement Threshold
- Increase `Move Threshold` for more deliberate movement
- Decrease for more responsive movement
- Useful for gamepad deadzone simulation

### Adding New Input Actions
1. Modify the Input Actions asset in `Assets/Settings/InputSystem_Actions.inputactions`
2. Update PlayerInputSystem to read the new actions
3. Add corresponding components if needed

### Visual Representation
- Replace the default sprite with your own 2D sprite
- Adjust the camera orthographic size for different zoom levels
- Add animations by extending the AnimationComponent
- Use SpriteRenderer components for 2D rendering

## System Selection

### For 2D Top-Down Games (Recommended)
- Use `Player2DMovementSystem` for enhanced 2D movement
- Better movement state tracking
- Configurable movement threshold
- Improved rotation handling

### For Simple Movement
- Use `PlayerMovementSystem` for basic movement
- Simpler, more straightforward
- Good for prototyping or simple games

## Troubleshooting

### Player Not Moving
1. Check if the player is in a SubScene
2. Verify PlayerAuthoring component is attached
3. Ensure Player2DMovementSystem (or PlayerMovementSystem) is running
4. Check console for any errors

### Input Not Working
1. Verify Input Actions asset is assigned
2. Check if PlayerInputSystem is running
3. Ensure input is enabled in PlayerAuthoring
4. Check that Input System package is installed

### Movement Too Sensitive
1. Increase the `Move Threshold` value in PlayerAuthoring
2. This prevents small input values from moving the player

### Performance Issues
- The systems use Burst compilation for optimal performance
- Consider using IJobEntity for very large numbers of players
- Player2DMovementSystem is optimized for 2D games 