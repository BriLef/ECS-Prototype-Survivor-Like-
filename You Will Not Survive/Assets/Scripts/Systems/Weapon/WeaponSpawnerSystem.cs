using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Components.Weapon;

namespace Systems.Weapon
{
    public partial class WeaponSpawnerSystem : SystemBase
    {
        
        protected override void OnUpdate()
        {
            // This system will be expanded to handle weapon spawning
            // For example, when a player levels up and gains a new weapon
        }
       
        public Entity SpawnWeapon(WeaponType weaponType, float damage, float fireRate, float maxActiveTime)
        {
            

            var weaponEntity = EntityManager.CreateEntity();
            
            EntityManager.AddComponentData(weaponEntity, new WeaponComponent
            {
                Type = weaponType,
                Damage = damage,
                FireRate = fireRate,
                LastFireTime = 0f,
                IsActive = false,
                ActiveTime = 0f,
                MaxActiveTime = maxActiveTime
            });
            
            switch (weaponType)
            {
                case WeaponType.RotatingOrb:
                    EntityManager.AddComponentData(weaponEntity, new RotatingWeaponComponent
                    {
                        RotationSpeed = 90f,
                        CurrentAngle = 0f,
                        Radius = 2f
                    });
                    break;
                    
                case WeaponType.RangedProjectile:
                case WeaponType.Firebolt:
                case WeaponType.IceSpike:
                    EntityManager.AddComponentData(weaponEntity, new ProjectileWeaponComponent
                    {
                        ProjectileSpeed = 10f,
                        MaxDistance = 15f,
                        SpreadAngle = 0f,
                        ProjectileCount = 1,
                        IsPiercing = false,
                        IsHoming = false
                    });
                    break;
                    
                case WeaponType.Lightning:
                    EntityManager.AddComponentData(weaponEntity, new ProjectileWeaponComponent
                    {
                        ProjectileSpeed = 15f,
                        MaxDistance = 12f,
                        SpreadAngle = 45f,
                        ProjectileCount = 3,
                        IsPiercing = true,
                        IsHoming = false
                    });
                    break;
                    
                case WeaponType.PoisonCloud:
                    EntityManager.AddComponentData(weaponEntity, new AreaOfEffectWeaponComponent
                    {
                        AreaRadius = 3f,
                        DamageOverTime = 5f,
                        TickRate = 1f,
                        LastTickTime = 0f
                    });
                    break;
                    
                case WeaponType.Whip:
                    EntityManager.AddComponentData(weaponEntity, new MeleeWeaponComponent
                    {
                        ExtensionDistance = 3f,
                        ArcAngle = 90f,
                        SweepSpeed = 180f
                    });
                    break;
            }
            
            EntityManager.AddComponentData(weaponEntity, new WeaponRendererComponent
            {
                OwnerEntity = Entity.Null,
                OffsetFromOwner = float2.zero
            });
            
            EntityManager.AddComponent<LocalTransform>(weaponEntity);
            
            return weaponEntity;
        }
        
        public void GiveWeaponToPlayer(Entity playerEntity, Entity weaponEntity)
        {
            if (!EntityManager.HasComponent<WeaponInventoryComponent>(playerEntity))
            {
                EntityManager.AddComponentData(playerEntity, new WeaponInventoryComponent
                {
                    Weapons = new Unity.Collections.FixedList512Bytes<Entity>()
                });
            }
            
            var inventory = EntityManager.GetComponentData<WeaponInventoryComponent>(playerEntity);
            inventory.AddWeapon(weaponEntity);
            EntityManager.SetComponentData(playerEntity, inventory);
            
            var weaponRenderer = EntityManager.GetComponentData<WeaponRendererComponent>(weaponEntity);
            weaponRenderer.OwnerEntity = playerEntity;
            EntityManager.SetComponentData(weaponEntity, weaponRenderer);
        }
    }
}