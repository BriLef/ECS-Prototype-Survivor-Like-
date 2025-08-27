using Unity.Entities;
using Unity.Collections;

namespace Components.Weapon
{
    public struct WeaponInventoryComponent : IComponentData
    {
        public FixedList512Bytes<Entity> Weapons; // Store weapon entities (up to 64 weapons)
        public bool HasWeapons => Weapons.Length > 0;
        
        public void AddWeapon(Entity weaponEntity)
        {
            if (Weapons.Length < Weapons.Capacity)
            {
                Weapons.Add(weaponEntity);
            }
        }
        
        public void RemoveWeapon(Entity weaponEntity)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] == weaponEntity)
                {
                    Weapons.RemoveAt(i);
                    break;
                }
            }
        }
        
        public void RemoveWeaponAtIndex(int index)
        {
            if (index >= 0 && index < Weapons.Length)
            {
                Weapons.RemoveAt(index);
            }
        }
        
        public Entity GetWeaponAtIndex(int index)
        {
            if (index >= 0 && index < Weapons.Length)
            {
                return Weapons[index];
            }
            return Entity.Null;
        }
        
        public int GetWeaponCount()
        {
            return Weapons.Length;
        }
        
        public void ClearAllWeapons()
        {
            Weapons.Clear();
        }
    }
}
