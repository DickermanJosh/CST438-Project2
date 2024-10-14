using System;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        // public InputHandler inputHandler;
        #region Adjustable Stats
        [Header("LAYERS")]
        public LayerMask playerLayer;

        [Header("Input Dead Zones")]
        [Tooltip("Minimum vertical input required to trigger movement"), Range(0.01f, 0.99f)]
        public float verticalDeadZoneThreshold = 0.3f;

        [Tooltip("Minimum horizontal input required to trigger movement"), Range(0.01f, 0.99f)]
        public float horizontalDeadZoneThreshold = 0.1f;

        [Header("Adjustable Movement Variables")]
        [Tooltip("Max horizontal speed")]
        public float maxSpeed = 12f;
        public float currentMaxSpeed;

        [Tooltip("Horizontal acceleration modifier")]
        public float acceleration = 75f;

        [Tooltip("The pace at which the player comes to a stop on the ground")]
        public float groundDeceleration = 90f;

        [Tooltip("Deceleration in air after stopping input")]
        public float airDeceleration = 30f;

        [Tooltip("A constant downward force applied while grounded"), Range(0f, -10f)]
        public float groundingForce = -1.5f;

        [Tooltip("The detection distance for ground and ceiling detection"), Range(0f, 0.5f)]
        public float groundCheckDistance = 0.05f;

        [Header("Adjustable Jump Variables")]
        [Tooltip("The immediate velocity applied when jumping")]
        public float jumpPower = 22f;

        [Tooltip("The maximum downward speed while falling")]
        public float maxFallSpeed = 30f;

        [Tooltip("The player's capacity to gain downward speed (gravity)")]
        public float fallAcceleration = 75f;

        [Tooltip("Gravity modifier when the jump button is released early")]
        public float jumpingGravityModifier = 2f;

        [Tooltip("Allow the player to jump even if they've just left the ground")]
        public float coyoteTime = 0.15f;

        [Tooltip("Allow the player 'queue' their next jump just before landing")]
        public float jumpBuffer = 0.15f;
        #endregion

        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        #region Interface

        public Vector2 FrameInput { get; private set; }
        public event Action<bool> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        #region Singleton

        public static PlayerMovement Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(PlayerMovement)) as PlayerMovement;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static PlayerMovement _instance;
        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            currentMaxSpeed = maxSpeed + DangerMeter.Instance.GetCurrentAmount();
        }

        #region Input Handling
        /*
         * Listen for event notifications from InputHandler and call the necessary functions
         */
        private void OnEnable()
        {
            if (InputHandler.Instance == null)
            {
                Debug.Log("InputHandler not found. (OneEnable)");
                return;
            }
            // Subscribe to input events
            InputHandler.Instance.OnJumpDown += OnJumpDown;
            InputHandler.Instance.OnJumpHeld += OnJumpHeld;
            InputHandler.Instance.OnJumpReleased += OnJumpReleased;
            InputHandler.Instance.OnMove += OnMove;
        }

        private void OnDisable()
        {
            if (InputHandler.Instance == null)
            {
                Debug.Log("InputHandler not found. (OnDisable)");
                return;
            }
            // Unsubscribe from input events
            InputHandler.Instance.OnJumpDown -= OnJumpDown;
            InputHandler.Instance.OnJumpHeld -= OnJumpHeld;
            InputHandler.Instance.OnJumpReleased -= OnJumpReleased;
            InputHandler.Instance.OnMove -= OnMove;
        }

        // Input variables for the current frame, toggled by the event listener
        private bool _jumpPressed;
        private bool _jumpHeld;
        private Vector2 _moveInput;

        // Input event handlers
        private void OnJumpDown()
        {
            _jumpPressed = true;
            _timeJumpWasPressed = _time;
        }

        private void OnJumpHeld()
        {
            _jumpHeld = true;
        }

        private void OnJumpReleased()
        {
            _jumpHeld = false;
            _endedJumpEarly = true;
        }

        private void OnMove(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }

        #endregion

        /*
         * Apply movement and collision checks at the end of the frame
         */
        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleHorizontalMovement();
            HandleGravity();

            ApplyMovement();

            // Reset per-frame jump press
            _jumpPressed = false;

            //TODO: Check direction of player
        }

        #region Collisions

        private float _lastTimeGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling detection
            var origin = (Vector2)_col.transform.position + _col.offset;
            var size = _col.size;

            var groundHit = Physics2D.CapsuleCast(
                _col.bounds.center,
                _col.size, _col.direction,
                0,
                Vector2.down,
                groundCheckDistance,
                ~playerLayer);
            
            var ceilingHit = Physics2D.CapsuleCast(
                _col.bounds.center,
                _col.size,
                _col.direction,
                0,
                Vector2.up,
                groundCheckDistance,
                ~playerLayer);

            // Ceiling collision
            if (ceilingHit)
            {
                _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }

            switch (_grounded)
            {
                // Grounded state change
                case false when groundHit:
                    _grounded = true;
                    _coyoteUsable = true;
                    _bufferedJumpUsable = true;
                    _endedJumpEarly = false;
                    GroundedChanged?.Invoke(true);
                    break;
                case true when !groundHit:
                    _grounded = false;
                    _lastTimeGrounded = _time;
                    GroundedChanged?.Invoke(false);
                    break;
            }

            // Update collider queries with the currently cached one
            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion

        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + jumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _lastTimeGrounded + coyoteTime;

        private void HandleJump()
        {
            // Queue up a jump to determine if it can be executed or not
            if (_jumpPressed)
            {
                _jumpToConsume = true;
            }

            if (_jumpHeld == false && _rb.velocity.y > 0 && !_endedJumpEarly && !_grounded)
            {
                _endedJumpEarly = true;
            }

            if (!_jumpToConsume && !HasBufferedJump)
            {
                return;
            }

            // Execute the jump if the player is either grounded or within the coyote limit
            if (_grounded || CanUseCoyote)
            {
                _endedJumpEarly = false;
                _timeJumpWasPressed = 0;
                _bufferedJumpUsable = false;
                _coyoteUsable = false;
                _frameVelocity.y = jumpPower;
                Jumped?.Invoke();
            }

            _jumpToConsume = false;
        }

        #endregion

        #region Horizontal Movement

        private void HandleHorizontalMovement()
        {
            // Apply dead zones to analog input
            var move = _moveInput;
            if (Mathf.Abs(move.x) < horizontalDeadZoneThreshold)
            {
                move.x = 0;
            }
            if (Mathf.Abs(move.y) < verticalDeadZoneThreshold)
            {
                move.y = 0;
            }

            FrameInput = move;

            // Horizontal movement logic
            if (move.x == 0)
            {
                // Decelerate if no input is given
                var deceleration = _grounded ? groundDeceleration : airDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                // Accelerate
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, move.x * currentMaxSpeed, acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                // Apply grounding force to keep player grounded
                _frameVelocity.y = groundingForce;
            }
            else
            {
                // Apply gravity when in the air
                var gravityMultiplier = fallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0)
                {
                    gravityMultiplier *= jumpingGravityModifier;
                }
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -maxFallSpeed, gravityMultiplier * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement()
        {
            _rb.velocity = _frameVelocity;
        }
    }
}