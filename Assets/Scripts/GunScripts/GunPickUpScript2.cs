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
    }
}
