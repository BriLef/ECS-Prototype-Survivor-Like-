using Unity.Entities;
using Unity.Mathematics;

namespace Components
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