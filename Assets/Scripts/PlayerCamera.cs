using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float senX;
    public float senY;

    public Transform orientation;
    public Transform playerTransform;
    
    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;
        
        _rotationY += mouseX;
        _rotationX -= mouseY;
        
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);  
        
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0.0f);
        orientation.rotation = Quaternion.Euler(0.0f, _rotationY, 0.0f);
        
        //manages character direction
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
        playerTransform.rotation = Quaternion.Euler(0, _rotationY, 0);
    }
}
