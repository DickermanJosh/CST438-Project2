using System;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
    
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
        private static readonly int Speed = Animator.StringToHash("Speed");

        #endregion

        private void OnEnable()
        {
            if (InputHandler.Instance == null)
            {
                return;
            }
            // Subscribe to input events
            InputHandler.Instance.OnJumpDown += OnJumpDown;
            InputHandler.Instance.OnMove += OnMove;
        }

        private void OnDisable()
        {
            if (InputHandler.Instance == null)
            {
                return;
            }
            // Unsubscribe from input events
            InputHandler.Instance.OnJumpDown -= OnJumpDown;
            InputHandler.Instance.OnMove -= OnMove;
        }
        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _animator = gameObject.GetComponent<Animator>();
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
        
        private void OnJumpDown()
        {
        }
        
        private void OnMove(Vector2 input)
        {
            _animator.SetFloat(Speed, Mathf.Abs(input.x));
        }
    }
}
