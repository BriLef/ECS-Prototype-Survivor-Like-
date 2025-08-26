using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public enum AIState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Flee,
        Dead
    }
    
    public struct AIBehaviour : IComponentData
    {
        public AIState CurrentState;
        public float DetectionRange;
        public float AttackRange;
        public float PatrolRadius;
        public float StateTimer;
        public float MaxStateTime;
        public bool IsAggressive;
        public float AggressionLevel;
        
        public AIBehaviour(float detectionRange, float attackRange, float patrolRadius, bool isAggressive)
        {
            CurrentState = AIState.Idle;
            DetectionRange = detectionRange;
            AttackRange = attackRange;
            PatrolRadius = patrolRadius;
            StateTimer = 0f;
            MaxStateTime = 5f;
            IsAggressive = isAggressive;
            AggressionLevel = isAggressive ? 1f : 0.5f;
        }
        
        public void UpdateStateTimer(float deltaTime)
        {
            StateTimer += deltaTime;
            if (StateTimer >= MaxStateTime)
            {
                // Reset timer and potentially change state
                StateTimer = 0f;
            }
        }
        
        public void ChangeState(AIState newState)
        {
            CurrentState = newState;
            StateTimer = 0f;
        }
        
        public bool ShouldAttack(float distanceToTarget)
        {
            return distanceToTarget <= AttackRange && IsAggressive;
        }
        
        public bool ShouldChase(float distanceToTarget)
        {
            return distanceToTarget <= DetectionRange && distanceToTarget > AttackRange;
        }
        
        public bool ShouldFlee(float distanceToTarget)
        {
            return distanceToTarget <= AttackRange && !IsAggressive;
        }
    }
} 