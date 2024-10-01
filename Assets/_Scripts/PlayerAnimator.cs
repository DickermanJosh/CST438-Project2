using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
    
        #region Singleton
        public static PlayerAnimator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(PlayerAnimator)) as PlayerAnimator;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static PlayerAnimator _instance;
        #endregion

        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            HandleDirectionChange();
        }
    
        /*
         * Flip the player's sprite when they change directions
         */
        private void HandleDirectionChange()
        {
            if (PlayerMovement.Instance.FrameInput.x != 0)
            {
                _spriteRenderer.flipX = PlayerMovement.Instance.FrameInput.x < 0;
            }
        }
    }
}
