using UnityEngine;
using Unity.Entities;
using Components.Player;

namespace Authoring.Player
{
    public class PlayerAnimationAuthoring : MonoBehaviour
    {
        [Header("Animation References")]
        public Animator playerAnimator;
        public SpriteRenderer playerSprite;
        
        [Header("Animation Parameters")]
        public string idleTriggerName = "Idle";
        public string walkTriggerName = "Walk";
        public string celebrateTriggerName = "Celebrate";
        public string dieTriggerName = "Die";
        
        private void Awake()
        {
            // Auto-find components if not assigned
            if (playerAnimator == null)
                playerAnimator = GetComponent<Animator>();
                
            if (playerSprite == null)
                playerSprite = GetComponent<SpriteRenderer>();
        }
        
        private void Update()
        {
            // This will be replaced by the ECS system
            // For now, just ensure components exist
        }
    }
}
