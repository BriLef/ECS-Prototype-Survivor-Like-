using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace Components
{
    public struct WeaponRendererComponent : IComponentData
    {
        // These will be set by the authoring component
        public Entity OwnerEntity; // The entity that owns this weapon (player)
        public float2 OffsetFromOwner; // Position offset from the owner
    }
}

// This component will be on the Unity side to store the actual renderer references
public class WeaponRendererAuthoring : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public Vector2 offsetFromOwner = Vector2.zero;
    
    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
    }
}
