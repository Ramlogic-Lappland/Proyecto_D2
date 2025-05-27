using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GunSystemRayCast : MonoBehaviour
{
    [Header("Input System Actions")]
    [SerializeField] private InputActionReference shoot;
    [SerializeField] private InputActionReference reload;
    [Header("Gun Settings")]
    [SerializeField] private int damage;
    [SerializeField] private int magazineSize;
    [SerializeField] private int bulletsPerTrigger;
    [SerializeField] private float range;   
    [SerializeField] private float fireRate;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float reloadTime;
    [SerializeField] private float spread;
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyType;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject bulletHole;
    [Header("Object Pooling")]
    [SerializeField] private int maxBulletHoles = 20;
    [SerializeField] private int maxMuzzleFlashes = 5;
    [SerializeField] private float bulletHoleLifetime = 5f;
    [SerializeField] private float muzzleFlashLifetime = 0.1f;

    private Queue<GameObject> _bulletHolePool = new Queue<GameObject>();
    private Queue<GameObject> _muzzleFlashPool = new Queue<GameObject>();
    

    public bool allowButtonHold = false;
    private RaycastHit _rayHit;
    
    private int _bulletsShot;
    private int _bulletsLeft;

    private bool _shooting;
    private bool _readyToShoot;
    private bool _reloading;

    private void Awake()
    {
        _bulletsLeft = magazineSize;
        _readyToShoot = true;
        

        for (var i = 0; i < maxBulletHoles; i++)
        {
            var bh = Instantiate(bulletHole);
            bh.SetActive(false);
            _bulletHolePool.Enqueue(bh);
        }
    

        for (var i = 0; i < maxMuzzleFlashes; i++)
        {
            var mf = Instantiate(muzzleFlash);
            mf.SetActive(false);
            _muzzleFlashPool.Enqueue(mf);
        }
    }

    private void OnEnable()
    {
        shoot.action.performed += OnShootPerformed;
        shoot.action.canceled += OnShootCanceled;
        reload.action.performed += OnReloadPerformed;
        
        shoot.action.Enable();
        reload.action.Enable();
    }

    private void OnDisable()
    {
        shoot.action.performed -= OnShootPerformed;
        shoot.action.canceled -= OnShootCanceled;
        reload.action.performed -= OnReloadPerformed;
        
        reload.action.Disable();
        shoot.action.Disable();
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (allowButtonHold)
        {
            _shooting = true;
            TryShoot();
        }
        else
            TryShoot();
    }
    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        if (allowButtonHold)
        {
            _shooting = false;
        }
    }
    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        if (_bulletsLeft < magazineSize && !_reloading)
        {
            Reload();
        }
    }
    private void TryShoot()
    {
        if (_readyToShoot && !_reloading && _bulletsLeft >= bulletsPerTrigger)  // Changed condition
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _readyToShoot = false;
    
        // Show muzzle flash once per trigger pull
        var flash = GetPooledObject(_muzzleFlashPool, muzzleFlash, attackPoint.position, Quaternion.identity);
        StartCoroutine(ReturnToPool(flash, _muzzleFlashPool, muzzleFlashLifetime));
    
        // Fire all pellets at once
        for (int i = 0; i < bulletsPerTrigger; i++)
        {
            FireSinglePellet();
        }
    
        _bulletsLeft -= bulletsPerTrigger;  // Deduct all bullets at once
    
        Invoke(nameof(ResetShoot), fireRate);  // Use fireRate for delay between shots
    }

    private void FireSinglePellet()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);
    
        if (Physics.Raycast(playerCamera.transform.position, direction, out _rayHit, range, enemyType))
        {
            var hole = GetPooledObject(
                _bulletHolePool,
                bulletHole,
                _rayHit.point + _rayHit.normal * 0.01f,
                Quaternion.LookRotation(_rayHit.normal)
            );
            StartCoroutine(ReturnToPool(hole, _bulletHolePool, bulletHoleLifetime));
        
            // Enemy damage logic would go here
            // if (_rayHit.collider.CompareTag("Enemy"))
            //    _rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
    }

    private void ResetShoot()
    {
        _readyToShoot = true;
    
        // Auto-fire if still holding trigger
        if (allowButtonHold && _shooting && _bulletsLeft >= bulletsPerTrigger)
        {
            TryShoot();
        }
    }
    private void Reload()
    {
        _reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        _bulletsLeft = magazineSize;
        _reloading = false;
    }
    
    private GameObject GetPooledObject(Queue<GameObject> pool, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
    
        // If pool is empty, create a new instance (optional)
        GameObject newObj = Instantiate(prefab, position, rotation);
        return newObj;
    }

    private IEnumerator ReturnToPool(GameObject obj, Queue<GameObject> pool, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
