using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Components.Player;
using Components.Input;

namespace Systems.Player
{
    [BurstCompile]
    public partial struct Player2DMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
                    foreach (var (transform, input, movement, player2D) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerInputComponent>, RefRO<PlayerMovementComponent>, RefRW<Player2DComponent>>())
            {
                if (!movement.ValueRO.CanMove)
                    continue;
                    
                var currentPosition = transform.ValueRO.Position;
                var moveDirection = input.ValueRO.MoveDirection;
                
                bool isMoving = math.lengthsq(moveDirection) > player2D.ValueRO.MoveThreshold;
                player2D.ValueRW.IsMoving = isMoving;
                
                if (isMoving)
                {
                    player2D.ValueRW.LastMoveDirection = moveDirection;

                    var newPosition = currentPosition + new float3(moveDirection.x, moveDirection.y, 0) * movement.ValueRO.MoveSpeed * deltaTime;
                    transform.ValueRW.Position = newPosition;
                    
                    // Update facing direction
                    if (math.lengthsq(moveDirection) > 0.01f)
                    {
                        player2D.ValueRW.FacingDirection = math.normalize(moveDirection);
                    }
                }
            }
        }
    } 
}