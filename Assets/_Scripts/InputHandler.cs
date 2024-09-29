using System;
using UnityEngine;

namespace _Scripts
{
    /*
    * Input manager axis to DuelSense inputs
   - 0 - Square
   - 1 - X
   - 2 - Circle
   - 3 - Triangle
   - 4 - Left Bumper
   - 5 - Right Bumper
   - 6 - Left Trigger
   - 7 - Right Trigger
   - 8 - Share Button
   - 9 - Menu Button
   - 10 - Left Stick Down
   - 11 - Right Stick Down
   - 12 - On / Off Button
   - 13 - DuelSense GamePad
    */

    public class InputHandler : MonoBehaviour
    {
        // Events for jumping and movement, subscribed to by PlayerMovement.cs
        public event Action OnJumpDown;
        public event Action OnJumpHeld;
        public event Action OnJumpReleased;
        public event Action<Vector2> OnMove;
        
        public static InputHandler Instance { get; private set; }
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            HandleJumpInputs();
            HandleMovementInputs();
        }

        private void HandleJumpInputs()
        {
            // Check for JumpDown input and invoke the event
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            {
                OnJumpDown?.Invoke();
            }

            // Check for JumpHeld input and invoke the event
            if (Input.GetButton("Jump") || Input.GetKey(KeyCode.Space))
            {
                OnJumpHeld?.Invoke();
            }
            
            // Check for JumpReleased input and invoke the event
            if (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Space))
            {
                OnJumpReleased?.Invoke();
            }
        }

        private void HandleMovementInputs()
        {
            var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // Invoke the movement event
            OnMove?.Invoke(moveInput);
        }
    }
}