using Unity.Entities;

namespace Components.Weapon
{
    public struct RotatingWeaponComponent : IComponentData
    {
        public float RotationSpeed; // Degrees per second
        public float CurrentAngle; // Current rotation angle
        public float Radius; // Distance from player center
    }
}