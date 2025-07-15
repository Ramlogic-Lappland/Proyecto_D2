using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input System Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference runAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Movement Speed/Force Settings & HP")]
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float runSpeed = 18f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float groundDistanceCheck = 2f; // how high you can get 
    [SerializeField] private float airControlMultiplier = 0.5f; // air speed modifier 
    [SerializeField] private float rayStartHeight = 0.2f; // a slight elevation to have a  margin between floor and character
    [Header("Ground Layer Reference")]
    [SerializeField] private LayerMask groundLayer;
    [Header("References")]
    [SerializeField] private Camera playerCam; 
    [SerializeField] private CapsuleCollider playerCapsuleCollider; 
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform playerCapsule;
    [SerializeField] private HealthBar healthBar;
    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle = 45f; 
    [SerializeField] private float slopeClimbForce = 15;
    [SerializeField] private float slopeRayLength = 1.5f;
    
    private Vector2 _moveInput;
    private Vector2 _velocity;
    private bool _isRunning;
    private bool _isJumpRequested; 
    private bool _isGrounded;
    private bool _readyToJump;
    
    /// <summary>
    /// Awake methos sets the base values for some of the player settings
    /// </summary>
    private void Awake()
    {
        rigidbody.freezeRotation = true;
        _readyToJump = true;
        rigidbody.freezeRotation = true;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rigidbody.sleepThreshold = 0.1f;
    }
    /// <summary>
    /// manages the new input system
    /// </summary>
    private void OnEnable()
    {
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        
        runAction.action.performed += OnRun;
        runAction.action.canceled += OnRun;
        
        jumpAction.action.started += OnJump;
        
        moveAction.action.Enable();
        runAction.action.Enable();
        jumpAction.action.Enable();
        
    }
    /// <summary>
    /// De subscribes the previously subscribed methods on "OnEnable"
    /// </summary>
    private void OnDisable()
    {
        
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        
        runAction.action.performed -= OnRun;
        runAction.action.canceled -= OnRun;
        
        jumpAction.action.started -= OnJump;
        
        moveAction.action.Disable();
        runAction.action.Disable();
       
        jumpAction.action.Disable();

    }
    /// <summary>
    /// Mostly manages the raycasting of character so it can know if its making contact with the floor
    /// </summary>
    private void Update()
    {
            var rayStart = playerCapsule.position + (Vector3.up * rayStartHeight);
        
            var targetSpeed = _isRunning ? runSpeed : walkSpeed; 
            _isGrounded = Physics.Raycast // checks distance from ground of player (The ray is cast from middle of the capsule)
            (
                rayStart,
                Vector3.down,
                rayStartHeight + groundDistanceCheck, // Total distance = start height + check distance
                groundLayer
            );

        SpeedControl();
    }
    /// <summary>
    /// Just calls characterMovement of a  fixed update
    /// </summary>
    private void FixedUpdate()
    {
        CharacterMovement();
    }
    /// <summary>
    /// Sets the direction the player is facing, manages its movement acceleration and also checks for jump request checking if the character meets the requirements to call for it.
    /// also manages te groundcheck also manages slopes
    /// </summary>
    private void CharacterMovement()
    {
        
        var targetSpeed = _isRunning ? runSpeed : walkSpeed;
        
        var cameraForward =  playerCam.transform.forward;
        var cameraRight = playerCam.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        var moveDirection = (cameraForward * _moveInput.y + cameraRight * _moveInput.x).normalized; // Calculate movement direction relative to camera
        
        if (_isGrounded)
        {
            if (Physics.Raycast(playerCapsule.position + Vector3.up * 0.1f, Vector3.down, out var slopeHit, slopeRayLength, groundLayer))
            {
                var slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
                
                if (slopeAngle > 0 && slopeAngle <= maxSlopeAngle)
                {
                    moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
                    
                    var slopeFactor = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
                    
                    rigidbody.AddForce(Vector3.up * slopeFactor * slopeClimbForce, ForceMode.Force);
                }
            }
        }
        else
        {
            moveDirection *= airControlMultiplier;
        }
    
        rigidbody.AddForce(moveDirection * targetSpeed, ForceMode.Force);

        if (_isJumpRequested && _isGrounded && _readyToJump)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(JumpReset), jumpCooldown);
        }
    }
    /// <summary>
    /// controls aceleration  depending on the state of Player at the moment of moving
    /// </summary>
    private void SpeedControl()
    {
        var targetSpeed = _isRunning ? runSpeed : walkSpeed; 
        var flatVel = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
        if (flatVel.magnitude > targetSpeed)
        {
            var limitedVelocity = flatVel.normalized * targetSpeed;
            rigidbody.linearVelocity = new Vector3(limitedVelocity.x, rigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }
    /// <summary>
    /// Jumps
    /// </summary>
    private void Jump()
    {
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            _isJumpRequested = false;
    }
    /// <summary>
    /// resets Jump availability  
    /// </summary>
    private void JumpReset()
    {
        _readyToJump = true;
    }
    /// <summary>
    /// detects walking input
    /// </summary>
    /// <param name="context"></param>
    private void OnMove (InputAction.CallbackContext context) // handles walk
    {
        Debug.Log(message:context.ReadValue<Vector2>());
        _moveInput = context.ReadValue<Vector2>();
    }
    /// <summary>
    /// detects sprinting input 
    /// </summary>
    /// <param name="context"></param>
    private void OnRun(InputAction.CallbackContext context) // handles run
    {
        _isRunning = context.ReadValueAsButton();
        Debug.Log("Sprint: " + _isRunning);
    }
    /// <summary>
    /// Detects jump input
    /// </summary>
    /// <param name="context"></param>
    private void OnJump (InputAction.CallbackContext context) // handles jump
    {
        _isJumpRequested = true; 
    }
    
    /// <summary>
    /// Manages player Health when damaged (methos for enemies to access to player health through their respective damage source
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        Debug.Log($"Player took {damage} damage!");
        GameManager.GameManagerInstance.PlayerHealth.TakeDamage(damage);
        healthBar.SetHealth(GameManager.GameManagerInstance.PlayerHealth.Health);
    }
    /// <summary>
    /// heals player
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int healing)
    {
        GameManager.GameManagerInstance.PlayerHealth.Heal(healing);
        healthBar.SetHealth(GameManager.GameManagerInstance.PlayerHealth.Health);
    }
}
