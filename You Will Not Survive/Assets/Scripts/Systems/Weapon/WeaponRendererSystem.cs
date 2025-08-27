using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Components.Weapon;

namespace Systems.Weapon
{
    public partial class WeaponRendererSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            // Update weapon positions and states based on their active state
            foreach (var (weapon, weaponTransform, renderer) in SystemAPI.Query<RefRO<WeaponComponent>, RefRW<LocalTransform>, RefRO<WeaponRendererComponent>>())
            {
                var weaponData = weapon.ValueRO;
                var weaponRendererData = renderer.ValueRO;
                
                // Update weapon position to follow owner when active
                if (weaponData.IsActive && SystemAPI.HasComponent<LocalTransform>(weaponRendererData.OwnerEntity))
                {
                    var ownerTransform = SystemAPI.GetComponent<LocalTransform>(weaponRendererData.OwnerEntity);
                    var newPosition = ownerTransform.Position + new float3(weaponRendererData.OffsetFromOwner.x, weaponRendererData.OffsetFromOwner.y, 0);
                    
                    // Update the weapon's transform position
                    var updatedTransform = weaponTransform.ValueRW;
                    updatedTransform.Position = newPosition;
                    weaponTransform.ValueRW = updatedTransform;
                }
            }
        }
    }
}
