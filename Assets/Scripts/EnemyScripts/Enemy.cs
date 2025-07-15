using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
public class Enemy : MonoBehaviour
{
    private ScoreManager _scoreManager;
    
    [Header("Navmesh Settings")]
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] private Transform  player;
    [SerializeField] private float walkPointRange;
    [SerializeField] private LayerMask defineGround;
    [SerializeField] private LayerMask  definePlayer;
    public Vector3 walkPoint;
    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject projectile;
    [SerializeField] private int enemyDamage;
    [Header("Enemy Health")]
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;
    private bool _playerIsInAttackRange, _playerIsInSightRange, _alreadyAttacked, _walkPointSet;
    public bool isDead = false;
    private IObjectPool<Enemy> _enemyPool;
    private Vector3 _lastPosition;
    private float _stuckTime;
    [Header("Pooling")]
    public bool isTutorialEnemy = false;

    public void SetPool(IObjectPool<Enemy> pool)
    {
        if (!isTutorialEnemy)
        {
            _enemyPool = pool;
        }
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        isDead = false;
        _scoreManager = GameObject.Find("TutorialCanvas").GetComponent<ScoreManager>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _lastPosition) < 0.1f)
        {
            _stuckTime += Time.deltaTime;
            if (_stuckTime > 3f) 
            {
                _walkPointSet = false; 
                _stuckTime = 0f;
            }
        }
        else
        {
            _stuckTime = 0f;
        }
        _lastPosition = transform.position;
        
        _playerIsInSightRange = Physics.CheckSphere(transform.position, sightRange, definePlayer);
        _playerIsInAttackRange = Physics.CheckSphere(transform.position, attackRange, definePlayer);

        if (!_playerIsInSightRange && !_playerIsInAttackRange) Patroling();
        if (_playerIsInSightRange && !_playerIsInAttackRange) Chase();
        if (_playerIsInSightRange && _playerIsInAttackRange) Attack();
    }

    private void Patroling()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet) agent.SetDestination(walkPoint);
        
        var distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 3f) _walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        var attempts = 0;
        var validPoint = false;
    
        while (!validPoint && attempts < 10)
        {
            var randomX = Random.Range(-walkPointRange, walkPointRange);
            var randomZ = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

 
            var path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, walkPoint, NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    _walkPointSet = true;
                    validPoint = true;
                }
            }
            attempts++;
        }
        if (!validPoint)
        {
            Debug.LogWarning("Failed to find a valid walk point!");
            _walkPointSet = false;
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!_alreadyAttacked)
        {
            // Instantiate projectile
            var projectileObj = Instantiate(projectile, transform.position, Quaternion.identity);
        
            // Get the rigidbody and set velocity
            var rb = projectileObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                var direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * 30f;
            }
        
            // Set damage amount (if your projectile has the script)
            var projectileScript = projectileObj.GetComponent<EnemyProjectile>();
            if (projectileScript != null)
            {
                projectileScript.damageAmount = enemyDamage; // Now works
            }

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        if (isTutorialEnemy)
        {
            _scoreManager.score += 100f;
            Destroy(gameObject);
        }
        else if (_enemyPool != null)
        {
            _enemyPool.Release(this);
        }
        else
        {
            _scoreManager.score += 100f;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
