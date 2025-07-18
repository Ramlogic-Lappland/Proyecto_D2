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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null && damageable.IsDamageable)
            {
                damageable.TakeDamage(damageAmount);
                Debug.Log($"Projectile hit {other.name} for {damageAmount} damage");
            }
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}