using Unity.Entities;
using Unity.Mathematics;

namespace Components.Player
{
    public struct PlayerAnimationRendererComponent : IComponentData
    {
        public bool ShouldFlipX;                 // Whether to flip sprite based on direction
        public float LastMoveDirection;          // Last movement direction for flipping
    }
}
