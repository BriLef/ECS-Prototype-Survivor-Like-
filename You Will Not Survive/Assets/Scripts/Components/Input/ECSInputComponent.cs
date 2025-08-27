using Unity.Entities;
using Unity.Mathematics;

namespace Components.Input
{
    // ECS input component that stores raw input data
    public struct ECSInputComponent : IComponentData
    {
        public float2 MoveInput;           // Raw movement input (-1 to 1)
        public float2 NormalizedMoveInput; // Normalized movement input
        public bool IsInitialized;         // Whether input system is ready
        public float InputThreshold;       // Minimum input threshold to register movement
    }

    // Input event component for broadcasting input changes
    public struct InputEventComponent : IComponentData
    {
        public float2 LastMoveInput;       // Previous frame's input
        public float2 CurrentMoveInput;    // Current frame's input
        public bool InputChanged;          // Whether input changed this frame
        public float Timestamp;            // When this input occurred
    }
}
