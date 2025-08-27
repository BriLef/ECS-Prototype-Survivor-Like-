using Unity.Entities;

namespace Components.Weapon
{
    public struct ProjectileWeaponComponent : IComponentData
    {
        public float ProjectileSpeed;
        public float MaxDistance; // How far the projectile can travel
        public float SpreadAngle; // For weapons that fire in a cone
        public int ProjectileCount; // For weapons that fire multiple projectiles
        public bool IsPiercing; // Whether projectiles can hit multiple enemies
        public bool IsHoming; // Whether projectiles track enemies
    }
}