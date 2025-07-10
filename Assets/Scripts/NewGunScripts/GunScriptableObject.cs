using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public GunType type;
    public ImpactType impactType;
    public new string name;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public ShootConfigScriptObj shootConfig;
    public TrailConfigurationScriptableObj trailConfig;
    
    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _model;
    private float _lastShotTime;
    private ParticleSystem _shootSystem;
    private ObjectPool<TrailRenderer> _trailPool;

    public void Shoot()
    {
        if (Time.time > shootConfig.fireRate + _lastShotTime)
        {
            _lastShotTime = Time.time;
            _shootSystem.Play();

            Vector3 shootDirection = _shootSystem.transform.forward + new Vector3
            (Random.Range(-shootConfig.spread.x, shootConfig.spread.x),
                Random.Range(-shootConfig.spread.y, shootConfig.spread.y),
                Random.Range(-shootConfig.spread.z, shootConfig.spread.z));

            shootDirection.Normalize();

            if (Physics.Raycast(_shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue,
                    shootConfig.hitMask))
            {
                _activeMonoBehaviour.StartCoroutine
                (PlayTrail
                     (_shootSystem.transform.position,
                      hit.point,
                      hit
                     )
                );
            }
            else
            {
                _activeMonoBehaviour.StartCoroutine
                (PlayTrail
                    (_shootSystem.transform.position,
                     _shootSystem.transform.position + (shootDirection * trailConfig.missDistance),
                     new RaycastHit()
                    ) 
                );
            }
        }
    }
    
    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this._activeMonoBehaviour = activeMonoBehaviour;
        _lastShotTime = 0;
        _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        
        _model = Instantiate(modelPrefab);
        _model.transform.SetParent(parent, false);
        _model.transform.localPosition = spawnPoint;
        _model.transform.localRotation = Quaternion.Euler(spawnRotation);
        
        _shootSystem = _model.GetComponent<ParticleSystem>();
    }

    private TrailRenderer CreateTrail()
    {
        var instance = new GameObject("Bullet Trail");
        var trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;
        
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        return trail;
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        var instance = _trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;
        
        var distance = Vector3.Distance(startPoint, endPoint);
        var remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01( remainingDistance / distance ) );
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;
            
            yield return null;
        }
        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            SurfaceManager.Instance.HandleImpact(hit.transform.gameObject, endPoint, hit.normal, impactType, 0);
        }
        
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        _trailPool.Release(instance);
    }
}
