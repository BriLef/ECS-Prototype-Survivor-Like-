using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Weapon : IComponentData
    {
        public float Damage;
        public float Range;
        public float FireRate;
        public float LastFireTime;
        public bool IsAutomatic;
        public int AmmoCount;
        public int MaxAmmo;
        
        public Weapon(float damage, float range, float fireRate, int maxAmmo)
        {
            Damage = damage;
            Range = range;
            FireRate = fireRate;
            LastFireTime = 0f;
            IsAutomatic = false;
            AmmoCount = maxAmmo;
            MaxAmmo = maxAmmo;
        }
        
        public bool CanFire(float currentTime)
        {
            return currentTime - LastFireTime >= 1f / FireRate && AmmoCount > 0;
        }
        
        public void Fire(float currentTime)
        {
            LastFireTime = currentTime;
            AmmoCount = math.max(0, AmmoCount - 1);
        }
        
        public void Reload()
        {
            AmmoCount = MaxAmmo;
        }
        
        public float GetAmmoPercentage()
        {
            return (float)AmmoCount / MaxAmmo;
        }
    }
} 