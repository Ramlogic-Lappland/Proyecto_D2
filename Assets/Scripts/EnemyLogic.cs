using UnityEngine;
using UnityEngine.AI;
public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform  player;
    [SerializeField] private LayerMask defineGround, definePlayer;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private GameObject projectile;
    [SerializeField] public int enemyHealth;
    [SerializeField] private int enemyDamage;
    private bool _playerIsInAttackRange, _playerIsInSightRange, _alreadyAttacked; 

    public Vector3 walkPoint;
    private bool _walkPointSet;
    [SerializeField] private float walkPointRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
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
        if (distanceToWalkPoint.magnitude < 1f) _walkPointSet = false;
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
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>(); //attack projectile
            rb.AddForce(transform.forward * 60f, ForceMode.Impulse);
            rb.AddForce(transform.up * 6f, ForceMode.Impulse);
            
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
    
    private void SearchWalkPoint()
    {
        var randomZ = Random.Range(-walkPointRange, walkPointRange);
        var randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, defineGround)) _walkPointSet = true;
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        
        if (enemyHealth <= 0 ) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
