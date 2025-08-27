using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Components.Input;

namespace Systems.Input
{
    [BurstCompile]
    public partial class ECSInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            // Read Unity input and convert to ECS data
            // This is a bridge between Unity input and ECS
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");
            
            float2 rawInput = new float2(horizontal, vertical);
            float2 processedInput = ProcessInput(rawInput, 0.1f); // 0.1 threshold
            
            // Update all entities with ECSInputComponent
            foreach (var (input, entity) in SystemAPI.Query<RefRW<ECSInputComponent>>().WithEntityAccess())
            {
                var inputData = input.ValueRO;
                
                // Update input data
                input.ValueRW = new ECSInputComponent
                {
                    MoveInput = rawInput,
                    NormalizedMoveInput = processedInput,
                    IsInitialized = true,
                    InputThreshold = 0.1f
                };
            }
        }

        // Method to process and normalize input
        private float2 ProcessInput(float2 rawInput, float threshold)
        {
            // Apply deadzone threshold
            if (math.lengthsq(rawInput) < threshold * threshold)
            {
                return float2.zero;
            }
            
            // Normalize input for consistent movement speed
            return math.normalize(rawInput);
        }

        // Method to check if input has changed significantly
        private bool HasInputChanged(float2 lastInput, float2 currentInput, float threshold)
        {
            float2 difference = currentInput - lastInput;
            return math.lengthsq(difference) > threshold * threshold;
        }
    }
}
