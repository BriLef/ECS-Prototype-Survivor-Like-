using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public enum AnimationState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Hurt,
        Die,
        Jump,
        Fall
    }
    
    public struct Animation : IComponentData
    {
        public AnimationState CurrentState;
        public AnimationState PreviousState;
        public float StateTimer;
        public float TransitionDuration;
        public bool IsPlaying;
        public float Speed;
        public bool IsLooping;
        public int CurrentFrame;
        public int TotalFrames;
        
        public Animation(AnimationState initialState, float speed = 1f, bool isLooping = true)
        {
            CurrentState = initialState;
            PreviousState = initialState;
            StateTimer = 0f;
            TransitionDuration = 0.1f;
            IsPlaying = true;
            Speed = speed;
            IsLooping = isLooping;
            CurrentFrame = 0;
            TotalFrames = 1;
        }
        
        public void UpdateAnimation(float deltaTime)
        {
            if (!IsPlaying) return;
            
            StateTimer += deltaTime * Speed;
            
            if (StateTimer >= 1f / TotalFrames)
            {
                NextFrame();
                StateTimer = 0f;
            }
        }
        
        public void NextFrame()
        {
            CurrentFrame++;
            if (CurrentFrame >= TotalFrames)
            {
                if (IsLooping)
                {
                    CurrentFrame = 0;
                }
                else
                {
                    CurrentFrame = TotalFrames - 1;
                    IsPlaying = false;
                }
            }
        }
        
        public void ChangeState(AnimationState newState)
        {
            if (CurrentState == newState) return;
            
            PreviousState = CurrentState;
            CurrentState = newState;
            StateTimer = 0f;
            CurrentFrame = 0;
            IsPlaying = true;
        }
        
        public bool IsInTransition()
        {
            return StateTimer < TransitionDuration;
        }
        
        public float GetProgress()
        {
            return (float)CurrentFrame / TotalFrames;
        }
        
        public void SetSpeed(float speed)
        {
            Speed = math.max(0f, speed);
        }
        
        public void Pause()
        {
            IsPlaying = false;
        }
        
        public void Resume()
        {
            IsPlaying = true;
        }
        
        public void Reset()
        {
            CurrentFrame = 0;
            StateTimer = 0f;
            IsPlaying = true;
        }
    }
} 