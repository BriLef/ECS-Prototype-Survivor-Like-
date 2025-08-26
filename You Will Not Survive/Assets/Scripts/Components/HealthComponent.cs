using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Health : IComponentData
    {
        public float CurrentHealth;
        public float MaxHealth;
        public bool IsDead;
        
        
        public Health(float maxHealth)
        {
            CurrentHealth = maxHealth;
            MaxHealth = maxHealth;
            IsDead = false;
        }
        
        public void TakeDamage(float damage)
        {
            CurrentHealth = math.max(0f, CurrentHealth - damage);
            IsDead = CurrentHealth <= 0f;
        }
        
        public void Heal(float amount)
        {
            CurrentHealth = math.min(MaxHealth, CurrentHealth + amount);
        }
        
        public float GetHealthPercentage()
        {
            return CurrentHealth / MaxHealth;
        }
    }
} 