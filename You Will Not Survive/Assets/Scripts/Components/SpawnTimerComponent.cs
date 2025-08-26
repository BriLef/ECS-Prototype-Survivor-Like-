using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct SpawnTimer : IComponentData
    {
        public float SpawnInterval;
        public float CurrentTimer;
        public int MaxSpawns;
        public int CurrentSpawnCount;
        public bool IsActive;
        public bool IsRepeating;
        
        public SpawnTimer(float spawnInterval, int maxSpawns = -1, bool isRepeating = true)
        {
            SpawnInterval = spawnInterval;
            CurrentTimer = 0f;
            MaxSpawns = maxSpawns;
            CurrentSpawnCount = 0;
            IsActive = true;
            IsRepeating = isRepeating;
        }
        
        public void UpdateTimer(float deltaTime)
        {
            if (!IsActive) return;
            
            CurrentTimer += deltaTime;
        }
        
        public bool ShouldSpawn()
        {
            if (!IsActive) return false;
            
            if (MaxSpawns > 0 && CurrentSpawnCount >= MaxSpawns)
                return false;
                
            return CurrentTimer >= SpawnInterval;
        }
        
        public void OnSpawn()
        {
            CurrentTimer = 0f;
            CurrentSpawnCount++;
            
            if (MaxSpawns > 0 && CurrentSpawnCount >= MaxSpawns)
            {
                IsActive = false;
            }
        }
        
        public void Reset()
        {
            CurrentTimer = 0f;
            CurrentSpawnCount = 0;
            IsActive = true;
        }
        
        public float GetProgress()
        {
            return CurrentTimer / SpawnInterval;
        }
        
        public bool IsFinished()
        {
            return MaxSpawns > 0 && CurrentSpawnCount >= MaxSpawns;
        }
    }
} 