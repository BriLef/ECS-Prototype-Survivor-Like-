using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Components.Player;
using Components.Weapon;
using Components.Input;

namespace Authoring.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAuthoring : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        
        [Header("2D Settings")]
        public float moveThreshold = 0.1f;
        public bool enableRotation = true;
        
        [Header("Input Settings")]
        public bool enableInput = true;
        
        [Header("Animation Settings")]
        public float celebrationDuration = 2f;
        public float deathAnimationDuration = 1.5f;
    }
    
    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<PlayerTagComponent>(entity);
            AddComponent(entity, new PlayerInputComponent(float2.zero));
            
            
            AddComponent(entity, new PlayerMovementComponent
            {
                MoveSpeed = authoring.moveSpeed,
                CanMove = authoring.enableInput,
                LastMoveDirection = float2.zero
            });
            

            AddComponent(entity, new Player2DComponent
            {
                MoveThreshold = authoring.moveThreshold,
                EnableRotation = authoring.enableRotation,
                IsMoving = false,
                LastMoveDirection = float2.zero,
                FacingDirection = new float2(0, 1) // Default facing up
            });
            
            // Add basic animation components (ECS only)
            AddComponent(entity, new PlayerAnimationComponent
            {
                CurrentState = Components.Player.AnimationState.Idle,
                StateTime = 0f,
                IsMoving = false,
                MoveThreshold = authoring.moveThreshold,
                IsCelebrating = false,
                CelebrationDuration = 2f,
                IsDead = false,
                DeathAnimationDuration = 1.5f
            });
            
            AddComponent(entity, new PlayerAnimationRendererComponent
            {
                ShouldFlipX = false,
                LastMoveDirection = 0f
            });
            
            // Add Unity GameObject reference (more stable than component references)
            AddComponentObject(entity, new PlayerAnimationUnityRefsComponent
            {
                PlayerGameObject = authoring.gameObject
            });
            Debug.Log("PlayerAuthoring: Successfully added PlayerAnimationUnityRefsComponent with GameObject reference");
            
            // Add weapon inventory component
            AddComponent(entity, new WeaponInventoryComponent
            {
                Weapons = new Unity.Collections.FixedList512Bytes<Entity>()
            });
        }
    }
}