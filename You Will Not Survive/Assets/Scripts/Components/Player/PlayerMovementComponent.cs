using Unity.Entities;
using Unity.Mathematics;

namespace Components.Player
{
    public struct PlayerMovementComponent : IComponentData
    {
        public float MoveSpeed;
        public bool CanMove;
        
        public PlayerMovementComponent(float moveSpeed = 5f)
        {
            MoveSpeed = moveSpeed;
            CanMove = true;
        }
    }
} 