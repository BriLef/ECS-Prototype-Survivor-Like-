using UnityEngine;
using Unity.Entities;
using Components;

public class PlayerSubSceneAuthoring : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float moveThreshold = 0.1f;
    public bool enableRotation = true;
    public bool enableInput = true;
}

public class PlayerSubSceneBaker : Baker<PlayerSubSceneAuthoring>
{
    public override void Bake(PlayerSubSceneAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        // Add all the player components
        AddComponent<PlayerTag>(entity);
        AddComponent<PlayerInputComponent>(entity);
        
        AddComponent(entity, new PlayerMovementComponent
        {
            MoveSpeed = authoring.moveSpeed,
            CanMove = authoring.enableInput
        });

        AddComponent(entity, new Player2DComponent
        {
            MoveThreshold = authoring.moveThreshold
        });
    }
}
