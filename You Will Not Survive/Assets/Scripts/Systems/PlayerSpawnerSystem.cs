using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization; // EntitySceneReference
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Scenes;
using UnityEngine;

public struct PlayerSpawnLoadState : IComponentData
{
    public Entity SceneEntity;
    public byte Phase; // 0=NotLoaded, 1=Waiting, 2=Done
}

[BurstCompile]
public partial struct PlayerSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerPrefabSceneRef>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var em = state.EntityManager;

        // Create state once
        if (!SystemAPI.TryGetSingleton<PlayerSpawnLoadState>(out var loadState))
        {
            var sceneRef = SystemAPI.GetSingleton<PlayerPrefabSceneRef>().Scene;

            var sceneEntity = SceneSystem.LoadSceneAsync(
                state.WorldUnmanaged,
                sceneRef,
                new SceneSystem.LoadParameters
                {
                    // Block on stream-in helps, but Live Baking can still finish next frame in Editor
                    Flags = SceneLoadFlags.LoadAdditive | SceneLoadFlags.BlockOnStreamIn
                }
            );

            em.CreateSingleton(new PlayerSpawnLoadState { SceneEntity = sceneEntity, Phase = 1 });
            return; // wait a frame
        }

        if (loadState.Phase == 1)
        {
            // Scene should be loaded; check if registry is there yet
            var q = em.CreateEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerPrefab>(),
                    ComponentType.ReadOnly<SceneTag>()
                }
            });
            q.SetSharedComponentFilter(new SceneTag { SceneEntity = loadState.SceneEntity });

            var count = q.CalculateEntityCount();
            q.ResetFilter();

            if (count == 0)
            {
                // Still baking — try next frame, no errors
                return;
            }

            // Registry exists: fetch it and spawn
            var regQ = em.CreateEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerPrefab>(),
                    ComponentType.ReadOnly<SceneTag>()
                }
            });
            regQ.SetSharedComponentFilter(new SceneTag { SceneEntity = loadState.SceneEntity });
            var regs = regQ.ToComponentDataArray<PlayerPrefab>(Allocator.Temp);
            var reg  = regs[0];
            regs.Dispose();
            regQ.ResetFilter();

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var player = ecb.Instantiate(reg.Value);
            ecb.SetComponent(player, LocalTransform.FromPositionRotationScale(new float3(0,0,0), quaternion.identity, 1f));
            ecb.AddComponent<PlayerTag>(player);
            ecb.Playback(em);
            ecb.Dispose();

            // Unload template scene
            SceneSystem.UnloadScene(state.WorldUnmanaged, loadState.SceneEntity);

            // Mark done & disable
            SystemAPI.SetSingleton(new PlayerSpawnLoadState { SceneEntity = Entity.Null, Phase = 2 });
            state.Enabled = false;
        }
    }
}