using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damageAmount = 10;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private DamageHandler damageHandler;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Hit: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collision confirmed");
            var damageable = collision.gameObject.GetComponentInParent<IDamageable>();
            if (damageable == null)
            {
                Debug.LogError("No IDamageable component found in hierarchy!");
                return;
            }
            Debug.Log($"IsDamageable: {damageable.IsDamageable}");
        
            if (damageable.IsDamageable)
            {
                if (damageHandler != null)
                {
                    Debug.Log($"Using damage handler: {damageHandler.GetType().Name}");
                    damageHandler.TakeDamage(damageable, damageAmount, 0);
                }
                else
                {
                    Debug.Log("Using direct damage");
                    damageable.TakeDamage(damageAmount);
                }
            }
            else
            {
                Debug.Log("Player is currently invincible");
            }

            Destroy(gameObject);
        }
    }
}