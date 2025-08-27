using Unity.Entities;
using Unity.Mathematics;

namespace Components.Weapon
{
    public struct WeaponRendererComponent : IComponentData
    {
        // These will be set by the authoring component
        public Entity OwnerEntity; // The entity that owns this weapon (player)
        public float2 OffsetFromOwner; // Position offset from the owner
    }
}