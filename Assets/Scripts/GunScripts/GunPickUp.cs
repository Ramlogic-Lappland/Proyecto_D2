using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GunPickUp : MonoBehaviour
{
    
    [Header("Input System Actions")]
    [SerializeField] private InputActionReference dropAction;
    [Header("References")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GunSystemRayCast gunScript;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider gunCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform playerCamera;
    [Header("PickUp Settings")]
    [SerializeField] private float pickUpRange;
    [SerializeField] private float dropForwardForce;
    [SerializeField] private float dropUpwardForce;

    public bool equipped;
    private static bool _slotFull;
    private void OnEnable()
    {
        dropAction.action.started += OnDrop; 
   
        dropAction.action.Enable();
    }
    private void OnDisable()
    {
        dropAction.action.started -= OnDrop; 
   
        dropAction.action.Disable();
    }

    private void Start()
    {
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            gunCollider.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            gunCollider.isTrigger = true;
        }
    }

    private void Update()
    {
        var distanceToPlayer = playerTransform.position - playerTransform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && !_slotFull) 
            PickUp();
    }
    private void PickUp()
    {
        equipped = true;
        _slotFull = true;
        
        transform.SetParent(gunHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        
        rb.isKinematic = true;
        gunCollider.isTrigger = true;
        
        gunScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        _slotFull = false;
        
        transform.SetParent(null);
        
        rb.isKinematic = false;
        gunCollider.isTrigger = false;
        
        rb.linearVelocity = playerTransform.GetComponent<Rigidbody>().linearVelocity;
        
        rb.AddForce(playerCamera.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(playerCamera.forward * dropUpwardForce, ForceMode.Impulse);
        var random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 4);
        gunScript.enabled = false;
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        if (equipped)
            Drop();
    }

}
