using Unity.Entities;

namespace Components.Weapon
{
    // For area of effect weapons
    public struct AreaOfEffectWeaponComponent : IComponentData
    {
        public float AreaRadius; // Radius of the effect area
        public float DamageOverTime; // For DoT effects
        public float TickRate; // How often DoT damage is applied
        public float LastTickTime;
    }
}