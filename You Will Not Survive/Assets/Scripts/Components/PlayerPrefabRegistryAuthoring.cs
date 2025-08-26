using Unity.Entities;
using UnityEngine;
using Components;

public struct PlayerTag : IComponentData {}     // put on the player prefab (and therefore on the instances)
public struct PlayerPrefab : IComponentData     // registry that holds the prefab entity
{
    public Entity Value;
}

public class PlayerPrefabRegistryAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefabGO; // assign the prefab asset from the Project window

    class Baker : Baker<PlayerPrefabRegistryAuthoring>
    {
        public override void Bake(PlayerPrefabRegistryAuthoring authoring)
        {
            Debug.Log("PlayerPrefabRegistryAuthoring Bake");
            var registry = GetEntity(TransformUsageFlags.None);
            var prefabEntity = GetEntity(authoring.PlayerPrefabGO, TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(prefabEntity);
            AddComponent(registry, new PlayerPrefab { Value = prefabEntity });
        }
    }
}
