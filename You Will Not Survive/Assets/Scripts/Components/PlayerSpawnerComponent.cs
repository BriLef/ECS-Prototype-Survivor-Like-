using Unity.Entities;
using Unity.Scenes;
using Unity.Mathematics;

namespace Components
{
    public struct PlayerSpawnerComponent : IComponentData
    {
        public Entity PlayerPrefab; // Alternative: direct prefab reference
        public bool ShouldSpawn;
        public float3 SpawnPosition;
        public bool UsePrefabSpawn; // Flag to choose between scene or prefab spawning
    }
}
