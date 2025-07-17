using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Random = UnityEngine.Random;

public class GunSystemInstance : MonoBehaviour
{
    [Header("Input System Actions")]
    [SerializeField] private InputActionReference shoot;
    [SerializeField] private InputActionReference reload;
    [Header("Gun Settings")]
    [SerializeField] private int magazineSize;
    [SerializeField] private int bulletsPerTrigger;
    [SerializeField] private float range;   
    [SerializeField] private float fireRate;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float reloadTime;
    [SerializeField] private float spread;
    [SerializeField] private float bulletForce;
    [SerializeField] private float bulletForceUp;
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject bullet;
    [SerializeField] private TextMeshProUGUI ammoDisplay;
    [Header("Object Pooling")]
    [SerializeField] private int bulletPoolSize = 20;
    [SerializeField] private int maxMuzzleFlashes = 5;
    [SerializeField] private float muzzleFlashLifetime = 0.1f;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    readonly Queue<GameObject> muzzleFlashPool = new Queue<GameObject>();
    
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
        
        for (int i = 0; i < bulletPoolSize; i++)
        {
            var newBullet = Instantiate(bullet);
            newBullet.SetActive(false);
            bulletPool.Enqueue(newBullet);
        }
        
        for (var i = 0; i < maxMuzzleFlashes; i++)
        {
            var mf = Instantiate(muzzleFlash);
            mf.SetActive(false);
            muzzleFlashPool.Enqueue(mf);
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

    private void Update()
    {
        if (ammoDisplay != null)
            ammoDisplay.SetText(_bulletsLeft / bulletsPerTrigger + "/" + magazineSize / bulletsPerTrigger); 
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

        var flash = GetPooledObject(muzzleFlashPool, muzzleFlash, attackPoint.position, Quaternion.identity);     // Show muzzle flash once per trigger pull
        StartCoroutine(ReturnToPool(flash, muzzleFlashPool, muzzleFlashLifetime));
    

        for (var i = 0; i < bulletsPerTrigger; i++)    // Code works like this to fire all pellets at once
        {
            FireSinglePellet();
        }
    
        _bulletsLeft -= bulletsPerTrigger;  // Decrease all bullets at once
    
        Invoke(nameof(ResetShoot), fireRate);  // Use fireRate for delay between shots
    }

    private void FireSinglePellet()
    {
        var rayOrigin = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).origin;
        var direction = GetSpreadDirection(rayOrigin);
        
        var currentBullet = GetPooledBullet();
        if (currentBullet == null) return; 

        currentBullet.transform.position = attackPoint.position;
        currentBullet.transform.rotation = Quaternion.LookRotation(direction);
        currentBullet.SetActive(true);
        
        var rb = currentBullet.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero; // Reset velocity
        rb.AddForce(direction.normalized * bulletForce, ForceMode.Impulse);
        rb.AddForce(playerCamera.transform.up * bulletForceUp, ForceMode.Impulse);
        
        StartCoroutine(ReturnBulletToPool(currentBullet));
    }
    
    private Vector3 GetSpreadDirection(Vector3 rayOrigin)
    {
        Vector3 targetPoint;
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    
        if (Physics.Raycast(ray, out var hit, range))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(range);
    
        var directionWithoutSpread = targetPoint - attackPoint.position;
        var x = Random.Range(-spread, spread);
        var y = Random.Range(-spread, spread);
    
        return directionWithoutSpread + new Vector3(x, y, 0);
    }
    private GameObject GetPooledBullet()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }
        
        var newBullet = Instantiate(bullet);
        return newBullet;
    }
    
    private IEnumerator ReturnBulletToPool(GameObject bullet)
    {
        yield return new WaitForSeconds(3f); 
    
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
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
            var obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
    
        // If pool is empty, create a new instance (optional)
        var newObj = Instantiate(prefab, position, rotation);
        return newObj;
    }

    private IEnumerator ReturnToPool(GameObject obj, Queue<GameObject> pool, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
