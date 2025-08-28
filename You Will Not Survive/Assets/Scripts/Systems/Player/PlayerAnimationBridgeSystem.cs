using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Components.Player;
using Components.Input;
using Authoring.Player;
using AnimationState = Components.Player.AnimationState;

namespace Systems.Player
{
    public partial class PlayerAnimationBridgeSystem : SystemBase
    {
        private bool hasLoggedDebugInfo = false;
        
        protected override void OnUpdate()
        {
            // This system runs on the main thread to update Unity components
            foreach (var (animation, renderer, transform, input, entity) in SystemAPI.Query<
                RefRO<PlayerAnimationComponent>, 
                RefRO<PlayerAnimationRendererComponent>,
                RefRO<LocalTransform>,
                RefRO<PlayerInputComponent>>()
                .WithEntityAccess())
            {
                var anim = animation.ValueRO;
                var rend = renderer.ValueRO;
                var pos = transform.ValueRO;
                var playerInput = input.ValueRO;
                
                // Log debug info once per entity
                if (!hasLoggedDebugInfo)
                {
                    Debug.Log($"=== PLAYER ANIMATION DEBUG INFO ===");
                    Debug.Log($"Entity ID: {entity.Index}");
                    Debug.Log($"Animation State: {anim.CurrentState}");
                    Debug.Log($"Is Moving: {anim.IsMoving}");
                    Debug.Log($"Move Direction: {playerInput.MoveDirection}");
                    Debug.Log($"Position: {pos.Position}");
                    hasLoggedDebugInfo = true;
                }
                
                // Get the managed component using GetComponentObject
                if (!EntityManager.HasComponent<PlayerAnimationUnityRefsComponent>(entity))
                {
                    Debug.LogError($"PlayerAnimationBridgeSystem: Entity {entity.Index} missing PlayerAnimationUnityRefsComponent");
                    continue;
                }
                
                var unityRefs = EntityManager.GetComponentObject<PlayerAnimationUnityRefsComponent>(entity);
                if (unityRefs == null)
                {
                    Debug.LogError($"PlayerAnimationBridgeSystem: PlayerAnimationUnityRefsComponent is null for entity {entity.Index}");
                    continue;
                }
                
                if (unityRefs.PlayerGameObject == null)
                {
                    Debug.LogError($"PlayerAnimationBridgeSystem: PlayerGameObject is null for entity {entity.Index}");
                    continue;
                }
                
                // Log GameObject info
                Debug.Log($"GameObject: {unityRefs.PlayerGameObject.name}");
                Debug.Log($"GameObject Active: {unityRefs.PlayerGameObject.activeInHierarchy}");
                
                // Log ALL components on the GameObject to see what's missing
                var allComponents = unityRefs.PlayerGameObject.GetComponents<Component>();
                Debug.Log($"GameObject has {allComponents.Length} components:");
                foreach (var comp in allComponents)
                {
                    if (comp != null)
                    {
                        Debug.Log($"  - {comp.GetType().Name}: {comp.name}");
                    }
                }
                
                var animator = unityRefs.PlayerGameObject.GetComponent<Animator>();
                var spriteRenderer = unityRefs.PlayerGameObject.GetComponent<SpriteRenderer>();
                
                if (animator == null)
                {
                    Debug.LogError($"PlayerAnimationBridgeSystem: Animator component missing on {unityRefs.PlayerGameObject.name}");
                    continue;
                }
                
                if (spriteRenderer == null)
                {
                    Debug.LogError($"PlayerAnimationBridgeSystem: SpriteRenderer component missing on {unityRefs.PlayerGameObject.name}");
                    continue;
                }
                
                // Log Animator info
                Debug.Log($"Animator Controller: {animator.runtimeAnimatorController?.name ?? "NULL"}");
                Debug.Log($"Animator Enabled: {animator.enabled}");
                Debug.Log($"Current State: {(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ? "Idle" : "Not Idle")}");
                
                // Add more detailed animator debugging
                var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                Debug.Log($"Current State Name: {(currentStateInfo.IsName("Idle") ? "Idle" : currentStateInfo.IsName("Walk") ? "Walk" : "Unknown")}");
                Debug.Log($"Current State Hash: {currentStateInfo.fullPathHash}");
                Debug.Log($"Current State Normalized Time: {currentStateInfo.normalizedTime}");
                Debug.Log($"Current State Length: {currentStateInfo.length}");
                Debug.Log($"Animator Speed: {animator.speed}");
                
                // Check sprite renderer
                Debug.Log($"Sprite Renderer Sprite: {spriteRenderer.sprite?.name ?? "NULL"}");
                Debug.Log($"Sprite Renderer Enabled: {spriteRenderer.enabled}");
                Debug.Log($"Sprite Renderer Color: {spriteRenderer.color}");
                Debug.Log($"Sprite Renderer Flip X: {spriteRenderer.flipX}");
                
                // Check if animator is actually changing states
                Debug.Log($"Animator Parameters Count: {animator.parameters.Length}");
                foreach (var param in animator.parameters)
                {
                    Debug.Log($"Parameter: {param.name} (Type: {param.type})");
                }
                
                // Check if player is moving based on input
                bool isMoving = math.lengthsq(playerInput.MoveDirection) > anim.MoveThreshold * anim.MoveThreshold;
                
                // Update Animator based on ECS state and input
                Debug.Log($"Bridge System - Current State: {anim.CurrentState}, IsMoving: {isMoving}, MoveDirection: {playerInput.MoveDirection}");
                
                // Always ensure idle animation is playing when not moving
                if (!isMoving && !anim.IsCelebrating && !anim.IsDead)
                {
                    animator.SetTrigger("Idle");
                    Debug.Log($"Set Idle trigger - Player not moving");
                }
                else if (isMoving && !anim.IsCelebrating && !anim.IsDead)
                {
                    animator.SetTrigger("Walk");
                    Debug.Log($"Set Walk trigger - Player moving");
                }
                
                // Handle special states
                switch (anim.CurrentState)
                {
                    case AnimationState.Celebrating:
                        animator.SetTrigger("Celebrate");
                        Debug.Log("Set Celebrate trigger");
                        break;
                    case AnimationState.Dying:
                        animator.SetTrigger("Die");
                        Debug.Log("Set Die trigger");
                        break;
                }
                
                // Handle sprite flipping
                if (rend.ShouldFlipX != spriteRenderer.flipX)
                {
                    spriteRenderer.flipX = rend.ShouldFlipX;
                }
                
                // Synchronize Unity GameObject position with ECS position
                var currentUnityPos = unityRefs.PlayerGameObject.transform.position;
                var ecsPos = pos.Position;
                
                if (Vector3.Distance(currentUnityPos, ecsPos) > 0.01f)
                {
                    unityRefs.PlayerGameObject.transform.position = ecsPos;
                    Debug.Log($"Bridge System - Synced position: ECS {ecsPos} -> Unity {unityRefs.PlayerGameObject.transform.position}");
                }
            }
        }
    }
}
