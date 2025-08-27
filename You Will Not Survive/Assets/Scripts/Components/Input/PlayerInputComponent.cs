using Unity.Entities;
using Unity.Mathematics;

namespace Components.Input
{
    public struct PlayerInputComponent : IComponentData
    {
        public float2 MoveDirection;

        public PlayerInputComponent(float2 moveDirection)
        {
            MoveDirection = moveDirection;
        }
    }
}