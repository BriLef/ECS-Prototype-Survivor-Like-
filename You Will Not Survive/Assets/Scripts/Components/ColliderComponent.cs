using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct Collider : IComponentData
    {
        public float Radius;
        public int Layer;
        public bool IsTrigger;
        public bool IsActive;
        
        
        public Collider(float radius, int layer, bool isTrigger = false)
        {
            Radius = radius;
            Layer = layer;
            IsTrigger = isTrigger;
            IsActive = true;
        }
        
        public bool CanCollideWith(int otherLayer)
        {
            // Simple collision layer check
            // Can be expanded with layer masks
            return IsActive && Layer != otherLayer;
        }
        
        public float GetArea()
        {
            return math.PI * Radius * Radius;
        }
        
        public void SetRadius(float radius)
        {
            Radius = math.max(0f, radius);
        }
    }
} 