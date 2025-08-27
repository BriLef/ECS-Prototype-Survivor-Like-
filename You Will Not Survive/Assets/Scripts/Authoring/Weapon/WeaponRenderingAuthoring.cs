using UnityEngine;
using Components.Weapon;

namespace Authoring.Weapon
{
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
}