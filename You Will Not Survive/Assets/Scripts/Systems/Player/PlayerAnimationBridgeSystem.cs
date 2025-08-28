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
        protected override void OnUpdate()
        {
            // This system runs on the main thread to update Unity components
            foreach (var (animation, renderer, transform, entity) in SystemAPI.Query<
                RefRO<PlayerAnimationComponent>, 
                RefRO<PlayerAnimationRendererComponent>,
                RefRO<LocalTransform>>()
                .WithEntityAccess())
            {
                var anim = animation.ValueRO;
                var rend = renderer.ValueRO;
                var pos = transform.ValueRO;
                
                // Get the managed component using GetComponentObject
                if (!EntityManager.HasComponent<PlayerAnimationUnityRefsComponent>(entity))
                {
                    Debug.LogWarning("PlayerAnimationBridgeSystem: Entity missing PlayerAnimationUnityRefsComponent");
                    continue;
                }
                
                var unityRefs = EntityManager.GetComponentObject<PlayerAnimationUnityRefsComponent>(entity);
                if (unityRefs == null || unityRefs.PlayerGameObject == null)
                {
                    Debug.LogWarning("PlayerAnimationBridgeSystem: PlayerAnimationUnityRefsComponent or PlayerGameObject is null");
                    continue;
                }
                
                var animator = unityRefs.PlayerGameObject.GetComponent<Animator>();
                var spriteRenderer = unityRefs.PlayerGameObject.GetComponent<SpriteRenderer>();
                
                if (animator == null || spriteRenderer == null) 
                {
                    Debug.LogWarning("PlayerAnimationBridgeSystem: Animator or SpriteRenderer is null");
                    continue;
                }
                
                // Update Animator based on ECS state
                Debug.Log($"Bridge System - Updating Animator to state: {anim.CurrentState}");
                switch (anim.CurrentState)
                {
                    case AnimationState.Idle:
                        animator.SetTrigger("Idle");
                        Debug.Log("Set Idle trigger");
                        break;
                    case AnimationState.Walking:
                        animator.SetTrigger("Walk");
                        Debug.Log("Set Walk trigger");
                        break;
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
