using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

namespace _Scripts
{
    public class InputHandler : MonoBehaviour
    {
        // Events for jumping and movement, subscribed to by PlayerMovement.cs
        public event Action OnJumpDown;
        public event Action OnJumpHeld;
        public event Action OnJumpReleased;
        public event Action<Vector2> OnMove;

        #region Singleton

        public static InputHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(InputHandler)) as InputHandler;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static InputHandler _instance;
        #endregion

        // Input Action class instance
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            // Initialize the input action class
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            // Enable the input actions
            _playerInputActions.Enable();
            
            // Subscribe to move and jump actions
            _playerInputActions.Player.Move.performed += OnMovePerformed;
            _playerInputActions.Player.Move.canceled += OnMoveCanceled;

            _playerInputActions.Player.Jump.started += OnJumpStarted;
            _playerInputActions.Player.Jump.performed += OnJumpHeldPerformed;
            _playerInputActions.Player.Jump.canceled += OnJumpCanceled;
            
            _playerInputActions.Player.Pause.performed += OnPausePerformed;
        }

        private void OnDisable()
        {
            // Unsubscribe from input actions when the object is disabled
            _playerInputActions.Player.Move.performed -= OnMovePerformed;
            _playerInputActions.Player.Move.canceled -= OnMoveCanceled;

            _playerInputActions.Player.Jump.started -= OnJumpStarted;
            _playerInputActions.Player.Jump.performed -= OnJumpHeldPerformed;
            _playerInputActions.Player.Jump.canceled -= OnJumpCanceled;
            
            _playerInputActions.Player.Pause.performed -= OnPausePerformed;

            // Disable the input actions
            _playerInputActions.Disable();
        }

        // Handle Move inputs
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Move");
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnMove?.Invoke(moveInput);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(Vector2.zero); // Stop movement when the input is canceled
        }

        // Handle Jump inputs
        private void OnJumpStarted(InputAction.CallbackContext context)
        {
            Debug.Log("Jump");
            OnJumpDown?.Invoke();
        }

        private void OnJumpHeldPerformed(InputAction.CallbackContext context)
        {
            OnJumpHeld?.Invoke();
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            OnJumpReleased?.Invoke();
        }
        
        public event Action OnPausePressed;

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Pause Pressed");
            OnPausePressed?.Invoke();
        }
        
        public void SwitchControlScheme(string schemeName)
        {
            // Find the control scheme by name
            var controlScheme = _playerInputActions.controlSchemes.FirstOrDefault(cs => cs.name == schemeName);

            // Set the binding mask to the selected control scheme
            _playerInputActions.bindingMask = InputBinding.MaskByGroups(controlScheme.bindingGroup);

            // Disable and re-enable the action maps to apply the new binding mask
            _playerInputActions.Disable();
            _playerInputActions.Enable();

            Debug.Log($"Switched to {schemeName} control scheme.");
        }
    }
}