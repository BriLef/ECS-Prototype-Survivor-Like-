using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Components.Player;
using Components.Input;
using UnityEngine;
using AnimationState = Components.Player.AnimationState;

namespace Systems.Player
{
    [BurstCompile]
    public partial class PlayerAnimationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (animation, renderer, input, transform) in SystemAPI.Query<
                RefRW<PlayerAnimationComponent>, 
                RefRW<PlayerAnimationRendererComponent>,
                RefRO<PlayerInputComponent>,
                RefRO<LocalTransform>>())
            {
                var anim = animation.ValueRW;
                var rend = renderer.ValueRW;
                
                // Check if player is moving (only if not celebrating or dying)
                float2 moveDirection = input.ValueRO.MoveDirection;
                bool isMoving = !anim.IsCelebrating && !anim.IsDead && 
                               math.lengthsq(moveDirection) > anim.MoveThreshold * anim.MoveThreshold;
                
                // Debug logging
                Debug.Log($"Player Animation - MoveDirection: {moveDirection}, IsMoving: {isMoving}, CurrentState: {anim.CurrentState}");
                
                // Update animation state
                if (isMoving != anim.IsMoving)
                {
                    anim.IsMoving = isMoving;
                    anim.StateTime = 0f; // Reset state time when changing states
                    
                    // Change animation state (only if not in special states)
                    if (!anim.IsCelebrating && !anim.IsDead)
                    {
                        if (isMoving)
                        {
                            anim.CurrentState = AnimationState.Walking;
                            // TODO: Tell Unity Animator to play walk animation
                        }
                        else
                        {
                            anim.CurrentState = AnimationState.Idle;
                            // TODO: Tell Unity Animator to play idle animation
                        }
                    }
                }
                
                // Update state time
                anim.StateTime += deltaTime;
                
                // Handle sprite flipping based on movement direction
                if (isMoving && math.lengthsq(moveDirection) > 0.01f)
                {
                    // Only change flip state when horizontal direction changes
                    float horizontalDirection = moveDirection.x;
                    
                    // If we have a significant horizontal movement, update the flip state
                    if (math.abs(horizontalDirection) > 0.1f)
                    {
                        bool shouldFlip = horizontalDirection < 0; // Flip if moving left
                        
                        if (shouldFlip != rend.ShouldFlipX)
                        {
                            rend.ShouldFlipX = shouldFlip;
                            rend.LastMoveDirection = math.atan2(moveDirection.y, moveDirection.x);
                            Debug.Log($"Changed flip state to: {shouldFlip} (moving {(shouldFlip ? "left" : "right")})");
                        }
                    }
                    // If moving primarily up/down, keep the current flip state
                    else
                    {
                        // Preserve the current flip state when moving up/down
                        Debug.Log($"Preserving flip state: {rend.ShouldFlipX} (moving up/down)");
                    }
                }
                
                // Update components
                animation.ValueRW = anim;
                renderer.ValueRW = rend;
            }
        }
        
        // Helper methods for other systems to trigger animations
        
        public void TriggerCelebration(Entity playerEntity, float duration = 2f)
        {
            if (SystemAPI.HasComponent<PlayerAnimationComponent>(playerEntity))
            {
                var anim = SystemAPI.GetComponent<PlayerAnimationComponent>(playerEntity);
                anim.IsCelebrating = true;
                anim.CelebrationDuration = duration;
                anim.CurrentState = AnimationState.Celebrating;
                anim.StateTime = 0f;
                SystemAPI.SetComponent(playerEntity, anim);
            }
        }
        
        public void TriggerDeath(Entity playerEntity, float animationDuration = 1.5f)
        {
            if (SystemAPI.HasComponent<PlayerAnimationComponent>(playerEntity))
            {
                var anim = SystemAPI.GetComponent<PlayerAnimationComponent>(playerEntity);
                anim.IsDead = true;
                anim.DeathAnimationDuration = animationDuration;
                anim.CurrentState = AnimationState.Dying;
                anim.StateTime = 0f;
                SystemAPI.SetComponent(playerEntity, anim);
            }
        }
        
        public void ResetToIdle(Entity playerEntity)
        {
            if (SystemAPI.HasComponent<PlayerAnimationComponent>(playerEntity))
            {
                var anim = SystemAPI.GetComponent<PlayerAnimationComponent>(playerEntity);
                anim.IsCelebrating = false;
                anim.IsDead = false;
                anim.IsMoving = false;
                anim.CurrentState = AnimationState.Idle;
                anim.StateTime = 0f;
                SystemAPI.SetComponent(playerEntity, anim);
            }
        }
    }
}
