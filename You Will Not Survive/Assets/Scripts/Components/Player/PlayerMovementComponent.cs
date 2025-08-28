using Unity.Entities;
using Unity.Mathematics;

namespace Components.Player
{
    public struct PlayerMovementComponent : IComponentData
    {
        public float MoveSpeed;
        public bool CanMove;
        public float2 LastMoveDirection;
    }
} 