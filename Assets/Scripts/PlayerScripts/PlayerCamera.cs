using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Sensibility")]
    [SerializeField] private float mouseSenX;
    [SerializeField] private float mouseSenY;
    [SerializeField] private float controllerSenX;
    [SerializeField] private float controllerSenY;
    [Header("Camera References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerTransform;
    [SerializeField]private InputActionReference lookReference; 
    
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private InputAction _inputAction;
    
    private void Awake()
    {
        _inputAction = lookReference.action;
        
        _inputAction.Enable();
    }
    private void OnDisable()
    {
        _inputAction?.Disable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        var lookInput = _inputAction.ReadValue<Vector2>();

        // Check if input is coming from a controller
        var isUsingController = Gamepad.current != null && lookInput.magnitude > 0.1f;

        // Choose sensitivity based on input device
        var sensX = isUsingController ? controllerSenX : mouseSenX;
        var sensY = isUsingController ? controllerSenY : mouseSenY;


        _rotationY += lookInput.x * sensX * Time.deltaTime;
        _rotationX -= lookInput.y * sensY * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);


        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
        orientation.rotation = Quaternion.Euler(0f, _rotationY, 0f);
        playerTransform.rotation = Quaternion.Euler(0f, _rotationY, 0f);
    }
}
