using System;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private LayerMask whatIsEnemy;
    
    [Header("Bounce System Attributes")]
    [Range(0f,1f)]
    [SerializeField] private float bounciness;
    [SerializeField] private bool useGravity;

    [Header("Explosion Attributes")]
    [SerializeField] private float rangeOfExplosion;
    [SerializeField] private int maxCollisions;

    [Header("Bullet Attributes")] 
    [SerializeField] private int bulletDamage;
    [SerializeField] private float maxLifeTime;
    [SerializeField] private bool explodeOnCollision = true;

    private int _collisions;
    private PhysicsMaterial _physicsMaterial;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (_collisions > maxCollisions) Explode();
        
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0) Explode();
    }

    private void Explode()
    {
        if (hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, rangeOfExplosion, whatIsEnemy);
        
        for (var i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].TryGetComponent<Player>(out var enemy)) enemy.TakeDamage(bulletDamage);
        }
        
        Destroy(gameObject, 0.5f);
    }

  

    private void OnCollisionEnter(Collision collision)
    {
        
        _collisions++;
        
        if (collision.collider.CompareTag("Enemy") && explodeOnCollision) Explode();
    }

    private void SetUp()
    {
        _physicsMaterial = new PhysicsMaterial();
        _physicsMaterial.bounciness = bounciness;
        _physicsMaterial.frictionCombine = PhysicsMaterialCombine.Minimum;
        _physicsMaterial.bounceCombine = PhysicsMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = _physicsMaterial;
        
        rb.useGravity = useGravity;
    }



}
