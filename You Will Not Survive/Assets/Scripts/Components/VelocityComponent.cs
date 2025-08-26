using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Velocity : IComponentData
    {
        public float3 Value;
        
        public Velocity(float3 velocity)
        {
            Value = velocity;
        }
        
        public void SetVelocity(float3 velocity)
        {
            Value = velocity;
        }
        
        public void AddVelocity(float3 delta)
        {
            Value += delta;
        }
        
        public void MultiplyVelocity(float multiplier)
        {
            Value *= multiplier;
        }
    }
} 