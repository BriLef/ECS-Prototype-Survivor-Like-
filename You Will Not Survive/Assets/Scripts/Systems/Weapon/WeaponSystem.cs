using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Components.Player;
using Components.Weapon;

namespace Systems.Weapon
{
    public partial class WeaponSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (weapon, transform, renderer) in SystemAPI.Query<RefRW<WeaponComponent>, RefRO<LocalTransform>, RefRO<WeaponRendererComponent>>())
            {
                var weaponRef = weapon.ValueRW;
                
                // Update active time for visible weapons
                if (weaponRef.IsActive)
                {
                    weaponRef.ActiveTime += deltaTime;
                    
                    // Hide weapon if it's been visible too long
                    if (weaponRef.ActiveTime >= weaponRef.MaxActiveTime)
                    {
                        weaponRef.IsActive = false;
                        weaponRef.ActiveTime = 0f;
                    }
                }
            }
            
            foreach (var (inventory, playerTransform) in SystemAPI.Query<RefRW<WeaponInventoryComponent>, RefRO<LocalTransform>>()
                .WithAll<PlayerTagComponent>())
            {
                // Check each weapon in the inventory
                for (int i = 0; i < inventory.ValueRO.Weapons.Length; i++)
                {
                    var weaponEntity = inventory.ValueRO.Weapons[i];
                    if (weaponEntity == Entity.Null) continue;
                    
                    // Check if we can fire this weapon
                    if (EntityManager.HasComponent<WeaponComponent>(weaponEntity))
                    {
                        var weapon = EntityManager.GetComponentData<WeaponComponent>(weaponEntity);
                        float currentTime = (float)SystemAPI.Time.ElapsedTime;
                        
                        // Check if fire rate allows firing
                        if (currentTime - weapon.LastFireTime >= 1f / weapon.FireRate)
                        {
                            // Fire the weapon
                            FireWeapon(ref weapon, playerTransform.ValueRO.Position);
                            
                            // Update the weapon component
                            EntityManager.SetComponentData(weaponEntity, weapon);
                        }
                    }
                }
            }
        }
        
        private void FireWeapon(ref WeaponComponent weapon, float3 playerPosition)
        {
            // Set weapon as active and visible
            weapon.IsActive = true;
            weapon.ActiveTime = 0f;
            weapon.LastFireTime = (float)SystemAPI.Time.ElapsedTime;    
        }
    }
}