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
    
    
    [SerializeField] private new Rigidbody rigidbody;
    //private Rigidbody rb;
    private Vector2 _moveInput;
    private Vector2 _velocity;
    
    private bool _isRunRequested;
    private bool _isJumpRequested; 
    private bool _isGrounded;


    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody>(); esta forma es mas segura ya que llama al rigid body del objeto que tenga el script y de esa forma evitas llmar al rigid body equivocado
        rigidbody.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
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
        
        jumpAction.action.performed -= OnJump;
    }
    
    private void FixedUpdate()
    {
        rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * walkSpeed , ForceMode.Force);
        if (_isJumpRequested)
        {
            _isJumpRequested = false;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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
        Debug.Log(message:context.ReadValue<Vector2>());
        _moveInput = context.ReadValue<Vector2>();
    }
    

}
