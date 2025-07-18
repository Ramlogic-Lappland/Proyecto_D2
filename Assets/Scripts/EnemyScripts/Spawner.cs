using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 5f;
    [SerializeField] private int enemyMaxHealth = 100;
    
    [Header("Tutorial Mode")]
    [SerializeField] private bool isTutorial = false;
    [SerializeField] private int tutorialEnemyCount = 1;
    
    private float _timeSinceLastSpawn;
    private IObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease);
    }
    private void Start()
    {
        if (isTutorial)
        {
            for (int i = 0; i < tutorialEnemyCount; i++)
            {
                SpawnTutorialEnemy();
            }
            this.enabled = false; 
        }
    }
    private void Update()
    {
        if (Time.time > _timeSinceLastSpawn)
        {
            SpawnEnemy();
            _timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
    }
    private void SpawnEnemy()
    {
        var enemy = _enemyPool.Get();
        var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;
        
        if (!enemy.agent.isOnNavMesh)
            enemy.agent.Warp(randomSpawnPoint.position);
    }
    private void SpawnTutorialEnemy()
    {
        var enemy = Instantiate(enemyPrefab); 
        enemy.transform.position = spawnPoints[0].position;
        
        enemy.isTutorialEnemy = true;
        
        if (!enemy.agent.isOnNavMesh)
            enemy.agent.Warp(spawnPoints[0].position);
    }
    private Enemy CreateEnemy()
    {
        var enemy = Instantiate(enemyPrefab);
        enemy.SetPool(_enemyPool);
        enemy.isTutorialEnemy = false;
        
        var health = enemy.GetComponent<Health>();
        if (health == null)
            health = enemy.gameObject.AddComponent<Health>();
        
        health.Initialize(enemyMaxHealth);
        return enemy;
    }
    private void OnGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.isDead = false;
        
        // Reset health
        var health = enemy.GetComponent<Health>();
        health?.Initialize(enemyMaxHealth);
        
        var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;
        
        if (!enemy.agent.isOnNavMesh)
            enemy.agent.Warp(randomSpawnPoint.position);
    }
    private void OnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
