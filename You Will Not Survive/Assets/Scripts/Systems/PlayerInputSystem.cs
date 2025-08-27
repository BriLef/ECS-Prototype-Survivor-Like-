using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Components;

public partial class PlayerInputSystem : SystemBase
{
    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private bool inputSystemInitialized = false;

    protected override void OnCreate()
    {
        // Create the Input Actions instance
        inputActions = new PlayerInputActions();
        TryInitializeInputSystem();
    }

    private void TryInitializeInputSystem()
    {
        try
        {
            if (inputActions != null)
            {
                // Get the Player action map directly
                var playerMap = inputActions.Player;
                
                // Get the Move action
                moveAction = playerMap.Move;
                if (moveAction != null)
                {
                    // Enable the action map
                    playerMap.Enable();
                    
                    inputSystemInitialized = true;
                    Debug.Log("PlayerInputSystem: New Input System initialized successfully");
                    return;
                }
                
            }
            
            Debug.LogWarning("PlayerInputSystem: Input Actions not configured, falling back to legacy input");
            inputSystemInitialized = false;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"PlayerInputSystem: Failed to initialize Input System: {e.Message}. Falling back to legacy input.");
            inputSystemInitialized = false;
        }
    }

    protected override void OnStopRunning()
    {
        // Disable the Player action map when the system stops running
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }

    protected override void OnDestroy()
    {
        // Final cleanup when the system is destroyed
        if (inputActions != null)
        {
            // Make sure Player is disabled before disposing
            inputActions.Player.Disable();
            
            // Dispose of the Input Actions asset
            inputActions.Dispose();
            inputActions = null;
        }
        
        // Clean up the move action if it was set
        if (moveAction != null)
        {
            moveAction.Dispose();
            moveAction = null;
        }
    }

    
    protected override void OnUpdate()
    {
        // Try to initialize again if it failed before (in case the asset was loaded later)
        if (!inputSystemInitialized)
        {
            TryInitializeInputSystem();
        }

        if (!inputSystemInitialized)
        {
            // Fallback to old input system if new input system isn't available
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            float2 moveInput = new float2(horizontal, vertical);
            
            // Normalize for consistent movement speed in all directions
            if (math.lengthsq(moveInput) > 0.01f)
            {
                moveInput = math.normalize(moveInput);
            }
            
            UpdatePlayerInput(moveInput);
        }
        else
        {
            // Use new input system
            try
            {
                Vector2 moveInput = moveAction.ReadValue<Vector2>();
                
                // Convert to float2 and normalize for consistent movement speed
                float2 normalizedInput = new float2(moveInput.x, moveInput.y);
                if (math.lengthsq(normalizedInput) > 0.01f)
                {
                    normalizedInput = math.normalize(normalizedInput);
                }
                
                UpdatePlayerInput(normalizedInput);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"PlayerInputSystem: Error reading from new Input System: {e.Message}. Falling back to legacy input.");
                inputSystemInitialized = false;
                
                // Fallback to legacy input
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                
                float2 moveInput = new float2(horizontal, vertical);
                if (math.lengthsq(moveInput) > 0.01f)
                {
                    moveInput = math.normalize(moveInput);
                }
                
                UpdatePlayerInput(moveInput);
            }
        }
    }

    private void UpdatePlayerInput(float2 moveDirection)
    {
        foreach (var(input, entity) in SystemAPI.Query<RefRW<PlayerInputComponent>>()
            .WithAll<PlayerTagComponent>()
            .WithEntityAccess())
        {
            input.ValueRW.MoveDirection = moveDirection;
        }
    }
}

