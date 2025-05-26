using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [Header("Input System Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference runAction;
    [SerializeField] private InputActionReference jumpAction;
    [Header("Movement Speed/Force Settings")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 25f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float groundDistanceCheck = 0.4f; // how high you can get 
    [SerializeField] private float airControlMultiplier = 0.5f; // air speed modifier 
    [SerializeField] private float rayStartHeight = 0.1f; // a slight elevation to have a  margin between floor and character
    [Header("Ground Layer Reference")]
    [SerializeField] private LayerMask groundLayer;
    [Header("References")]
    [SerializeField] private Camera playerCam; 
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform playerCapsule;

    private Vector2 _moveInput;
    private Vector2 _velocity;
    
    private bool _isRunning;
    private bool _isJumpRequested; 
    private bool _isGrounded;
    private bool _readyToJump;
    
    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody>(); this way is more secure of making sure you are referencing the correct rigidBody 
        rigidbody.freezeRotation = true;
        _readyToJump = true;
    }
    
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

    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        
        runAction.action.performed -= OnRun;
        runAction.action.canceled -= OnRun;
        
        jumpAction.action.started -= OnJump;
    }
    
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
        
        Debug.DrawRay(rayStart, Vector3.down * (rayStartHeight + groundDistanceCheck), 
            _isGrounded ? Color.green : Color.red);
    }
    
    private void FixedUpdate()
    {
        CharacterMovement();
    }

    private void CharacterMovement()
    {
        
        var targetSpeed = _isRunning ? runSpeed : walkSpeed;
        
        var cameraForward =  playerCam.transform.forward;
        var cameraRight = playerCam.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
    
        // Calculate movement direction relative to camera
        var moveDirection = (cameraForward * _moveInput.y + cameraRight * _moveInput.x).normalized;

        if (!_isGrounded)
            moveDirection *= airControlMultiplier;
        
        // rigidbody.AddForce(moveDirection * targetSpeed, ForceMode.Force);
        
        var horizontalVelocity = new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
        if (horizontalVelocity.magnitude < targetSpeed)
        {
            rigidbody.AddForce(moveDirection * targetSpeed, ForceMode.Force);
        }

        if (_isJumpRequested && _isGrounded && _readyToJump)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(JumpReset), jumpCooldown);
        }
    }
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
    private void Jump()
    {
            rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            _isJumpRequested = false;
    }
    private void JumpReset()
    {
        _readyToJump = true;
    }
    private void OnJump (InputAction.CallbackContext context) // handles jump
    {
        _isJumpRequested = true; 
    }
    
    private void OnMove (InputAction.CallbackContext context) // handles walk
    {
        Debug.Log(message:context.ReadValue<Vector2>());
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnRun(InputAction.CallbackContext context) // handles run
    {
        _isRunning = context.ReadValueAsButton();
        Debug.Log("Sprint: " + _isRunning);
    }

}
