using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [Header("enemy prefab")]
    [SerializeField] private Enemy enemyPrefab;
    [Header("spawners")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns;
    private float _timeSinceLastSpawn;
    private IObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease);
    }

    private void OnGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.health = enemy.maxHealth; 
        enemy.isDead = false;
        
        var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;
        if (!enemy.agent.isOnNavMesh)
            enemy.agent.Warp(randomSpawnPoint.position);
    }
    private void OnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Time.time > _timeSinceLastSpawn)
        {
            _enemyPool.Get();
            _timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
    }
    private Enemy CreateEnemy()
    {
        var enemy = Instantiate(enemyPrefab);
        enemy.SetPool(_enemyPool);
        return enemy;
    }
}
