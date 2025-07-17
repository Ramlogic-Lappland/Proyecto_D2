using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GunSystemRayCast gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool SlotFull;

    private void Start()
    {
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
                coll.isTrigger = true;
            SlotFull = true;
        }
    }

    private void Update()
    {
        var distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !SlotFull) PickUp();
        
        if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    private void PickUp()
    {
        equipped = true;
        SlotFull = true;
        rb.isKinematic = true;
        coll.isTrigger = true;
        gunScript.enabled = true;
        
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        Physics.IgnoreCollision(coll, player.GetComponent<Collider>(), true);
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.SetParent(null); 
        transform.position = gunContainer.position;
        transform.rotation = gunContainer.rotation;
        transform.SetParent(gunContainer);
    }

    private void Drop()
    {
        gunScript.enabled = false;
        equipped = false;
        SlotFull = false;
        rb.isKinematic = false;
        coll.isTrigger = false;

        transform.SetParent(null);
        
        rb.linearVelocity = player.GetComponent<Rigidbody>().linearVelocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        
        var random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
        var playerRb = player.GetComponent<Rigidbody>();
        var playerVelocity = playerRb != null ? playerRb.linearVelocity : Vector3.zero;
        
        rb.linearVelocity = playerVelocity * 0.5f; 
        rb.AddForce(fpsCam.forward * (dropForwardForce * 0.3f), ForceMode.Impulse); 
        rb.AddForce(fpsCam.up * (dropUpwardForce * 0.2f), ForceMode.Impulse);
        
        rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2f);
        Physics.IgnoreCollision(coll, player.GetComponent<Collider>(), false);
    }
}
