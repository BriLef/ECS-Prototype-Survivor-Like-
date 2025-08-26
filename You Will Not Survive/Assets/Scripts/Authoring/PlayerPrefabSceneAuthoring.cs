using Unity.Entities;
using Unity.Entities.Serialization; // EntitySceneReference
using UnityEngine;

public class PlayerPrefabSceneAuthoring : MonoBehaviour
{
    public EntitySceneReference PlayerPrefabScene;

    class Baker : Baker<PlayerPrefabSceneAuthoring>
    {
        public override void Bake(PlayerPrefabSceneAuthoring authoring)
        {
            Debug.Log("PlayerPrefabSceneAuthoring Bake");
            var e = GetEntity(TransformUsageFlags.None);
            AddComponent(e, new PlayerPrefabSceneRef { Scene = authoring.PlayerPrefabScene });
        }
    }
}

public struct PlayerPrefabSceneRef : IComponentData
{
    public EntitySceneReference Scene;
}
