using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject impactEffect;
    
    private void Start()
    {
        Destroy(gameObject, lifetime); // Auto-destroy after time
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if hit the player
        if (other.CompareTag("Player"))
        {
            // Apply damage to player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
            
            // Create impact effect
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject); // Destroy projectile on hit
        }
        // Optional: Add other collision cases (walls, etc.)
        else if (!other.CompareTag("Enemy")) // Don't destroy on enemy hits
        {
            Destroy(gameObject);
        }
    }
}