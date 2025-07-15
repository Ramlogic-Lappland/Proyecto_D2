using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damageAmount = 10;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject impactEffect;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
            
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}