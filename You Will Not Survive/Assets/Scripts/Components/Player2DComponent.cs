using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    /// <summary>
    /// Component for 2D top-down player specific functionality
    /// </summary>
    public struct Player2DComponent : IComponentData
    {
        public float2 LastMoveDirection;        // Last direction the player was moving
        public float2 FacingDirection;          // Current facing direction
        public bool IsMoving;                   // Whether the player is currently moving
        public float MoveThreshold;             // Minimum input threshold to consider movement
        
        public Player2DComponent(float moveThreshold = 0.1f)
        {
            LastMoveDirection = float2.zero;
            FacingDirection = new float2(0, 1); // Default facing up
            IsMoving = false;
            MoveThreshold = moveThreshold;
        }
    }
} 