using Unity.Entities;
using Unity.Mathematics;

namespace Components.Weapon
{
    public enum WeaponType
    {
        Whip,           // Melee weapon that extends in front of player
        RotatingOrb,    // Orb that rotates around player
        RangedProjectile, // Projectile that travels in a direction
        Firebolt,       // Magic projectile with special properties
        Lightning,      // Chain lightning or area damage
        IceSpike,       // Piercing projectile
        PoisonCloud,    // Area of effect damage over time
        HolyBeam        // Beam weapon
    }

    // Core weapon data that all weapons share
    public struct WeaponComponent : IComponentData
    {
        public WeaponType Type;
        public float Damage;
        public float FireRate; // Shots per second
        public float LastFireTime; // Track when weapon was last fired
        public bool IsActive; // Whether weapon is currently active/visible
        public float ActiveTime; // How long weapon stays visible
        public float MaxActiveTime; // Maximum time weapon can stay visible
    }
}
