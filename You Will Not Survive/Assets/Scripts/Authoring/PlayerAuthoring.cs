using UnityEngine;
using Unity.Entities;
using Components;

public class PlayerAuthoring : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    [Header("2D Settings")]
    public float moveThreshold = 0.1f;
    public bool enableRotation = true;
    
    [Header("Input Settings")]
    public bool enableInput = true;
}
 
public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent<PlayerTagComponent>(entity);
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
