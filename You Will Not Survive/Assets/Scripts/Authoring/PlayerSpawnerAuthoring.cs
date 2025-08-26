using UnityEngine;
using Unity.Entities;
using Unity.Scenes;
using Components;

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    [Header("Player Spawn Settings")]
    public GameObject playerPrefab; // Alternative: direct prefab reference
    public Vector3 spawnPosition = Vector3.zero;
    public bool spawnOnStart = true;

    public class PlayerSpawnerBaker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            Entity playerPrefab = Entity.Null;
            if (authoring.playerPrefab != null)
            {
                playerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic);
            }
            
            AddComponent(entity, new PlayerSpawnerComponent
            {
                PlayerPrefab = playerPrefab,
                ShouldSpawn = authoring.spawnOnStart,
                SpawnPosition = authoring.spawnPosition
            });
        }
    }
}
