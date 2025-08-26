using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Projectile : IComponentData
    {
        public float Speed;
        public float Lifetime;
        public float CurrentLifetime;
        public float Damage;
        public float3 Direction;
        public bool IsActive;
        
        public Projectile(float speed, float lifetime, float damage, float3 direction)
        {
            Speed = speed;
            Lifetime = lifetime;
            CurrentLifetime = 0f;
            Damage = damage;
            Direction = direction;
            IsActive = true;
        }
        
        public void UpdateLifetime(float deltaTime)
        {
            CurrentLifetime += deltaTime;
            if (CurrentLifetime >= Lifetime)
            {
                IsActive = false;
            }
        }
        
        public bool IsExpired()
        {
            return CurrentLifetime >= Lifetime;
        }
        
        public float GetRemainingLifetime()
        {
            return math.max(0f, Lifetime - CurrentLifetime);
        }
    }
} 