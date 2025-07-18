using UnityEngine;
using System.Collections;

public enum BossPhase { Phase1, Phase2, Phase3 }

public class PadoruScript : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1000;
    public int currentHealth;

    [Header("Invulnerability")]
    public bool isInvulnerable = false;
    public float invulnerabilityDuration = 0.5f;

    [Header("Phase Settings")]
    public int phase2HealthThreshold = 600;
    public int phase3HealthThreshold = 300;
    
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;
    [Range(0.1f, 2f)] public float fireRate = 0.5f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; 

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 30f;
    public bool alternateRotation = false;
    public float rotationChangeInterval = 5f;

    [Header("Bullet Spread")]
    public bool useSpread = false;
    public float spreadAngle = 15f;

    [Header("Phase Behavior")]
    public BossPhase currentPhase = BossPhase.Phase1;
    public float phase2FireRate = 0.3f;
    public float phase2RotationSpeed = 45f;
    public float phase3FireRate = 0.15f;
    public float phase3RotationSpeed = 60f;

    private float fireTimer;
    private float rotationTimer;
    private int currentRotationDirection = 1;

    void Start()
    {
        currentHealth = maxHealth;
        
        fireTimer = fireRate;
        rotationTimer = rotationChangeInterval;
        currentRotationDirection = alternateRotation ? -1 : 1;
    }

    void Update()
    {
        if (currentHealth > 0)
        {
            RotateBoss();
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                FireBullets();
                fireTimer = fireRate;
            }
        }
    }

    // HEALTH 
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        
        CheckPhases();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityFrame());
        }
    }

    IEnumerator InvulnerabilityFrame()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
    void CheckPhases()
    {
        if (currentHealth <= phase3HealthThreshold && currentPhase != BossPhase.Phase3)
        {
            SetPhase(BossPhase.Phase3);
        }
        else if (currentHealth <= phase2HealthThreshold && currentPhase != BossPhase.Phase2)
        {
            SetPhase(BossPhase.Phase2);
        }
    }

    void Die()
    {
        if (GetComponent<Collider>() != null) 
            GetComponent<Collider>().enabled = false;

        Destroy(gameObject, 3f);
    }

    // BOSS BEHAVIOR 
    public void SetPhase(BossPhase newPhase)
    {
        currentPhase = newPhase;
    
        switch (currentPhase)
        {
            case BossPhase.Phase2:
                fireRate = phase2FireRate;
                rotationSpeed = phase2RotationSpeed;
                rotationAxis = new Vector3(0, 1, 0.5f).normalized; 
                break;
            
            case BossPhase.Phase3:
                fireRate = phase3FireRate;
                rotationSpeed = phase3RotationSpeed;
                rotationAxis = new Vector3(1, 1, 1).normalized; 
                spreadAngle = 30f;
                break;
        }
    
        Debug.Log($"Entering {currentPhase}!");
    }
    void RotateBoss()
    {

        transform.Rotate(rotationAxis, rotationSpeed * currentRotationDirection * Time.deltaTime);
        if (alternateRotation)
        {
            rotationTimer -= Time.deltaTime;
            if (rotationTimer <= 0)
            {
                currentRotationDirection *= -1;
                rotationTimer = rotationChangeInterval;
            }
        }
    }
    void FireBullets()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            var bulletRotation = spawnPoint.rotation;
            
            if (useSpread)
            {
                var euler = bulletRotation.eulerAngles;
                euler.x += Random.Range(-spreadAngle, spreadAngle);
                euler.y += Random.Range(-spreadAngle, spreadAngle);
                bulletRotation = Quaternion.Euler(euler);
            }
            
            var bullet = Instantiate(bulletPrefab, spawnPoint.position, bulletRotation);

            var rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bullet.transform.forward * bulletSpeed;
            }
            
            Destroy(bullet, bulletLifetime);
        }
    }
}