using Unity.Entities;
using Unity.Mathematics;

namespace Components.Player
{
    public struct Player2DComponent : IComponentData
    {
        public float MoveThreshold;
        public bool EnableRotation;
        public bool IsMoving;
        public float2 LastMoveDirection;
        public float2 FacingDirection;
    }
} 