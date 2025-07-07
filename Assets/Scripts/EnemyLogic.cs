using UnityEngine;
using UnityEngine.AI;
public class EnemyLogic : MonoBehaviour
{
    [Header("Navmesh Settings")]
    [SerializeField] private NavMeshAgent agent;
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
    
    private bool _playerIsInAttackRange, _playerIsInSightRange, _alreadyAttacked, _isDead, _walkPointSet; 

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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
        var randomX = Random.Range(-walkPointRange, walkPointRange);
        var randomZ = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, defineGround)) _walkPointSet = true;
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
            var rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 30f, ForceMode.Impulse);
            rb.AddForce(transform.up * 6f, ForceMode.Impulse);
            
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
        if (health <= 0)
        {
            _isDead = true;
        }
        
        if (_isDead == true ) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
