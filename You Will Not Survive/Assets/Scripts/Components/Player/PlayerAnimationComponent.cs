using Unity.Entities;
using Unity.Mathematics;

namespace Components.Player
{
    public enum AnimationState
    {
        Idle,
        Walking,
        Celebrating,
        Dying
    }

    public struct PlayerAnimationComponent : IComponentData
    {
        public AnimationState CurrentState;      // Idle, Walking, Celebrating, Dying
        public float StateTime;                 // How long in current state
        public bool IsMoving;                   // Whether player is currently moving
        public float MoveThreshold;             // Minimum movement to trigger walk animation
        
        // For celebrating animations
        public bool IsCelebrating;              // Whether player is celebrating
        public float CelebrationDuration;       // How long celebration lasts
        
        // For dying animations
        public bool IsDead;                     // Whether player is dead
        public float DeathAnimationDuration;    // How long death animation takes
    }
}
