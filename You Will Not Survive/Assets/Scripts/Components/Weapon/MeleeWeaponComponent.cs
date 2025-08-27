using Unity.Entities;   

namespace Components.Weapon
{
    public struct MeleeWeaponComponent : IComponentData
    {
        public float ExtensionDistance; // How far the weapon extends
        public float ArcAngle; // Arc of attack (for sweeping weapons)
        public float SweepSpeed; // How fast the weapon sweeps
    }
}