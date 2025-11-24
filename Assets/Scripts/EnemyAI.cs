using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State state = State.Patrol;

    [Header("Refs")]
    public Transform player;                 
    public NavMeshAgent agent;             

    [Header("Perception")]
    public float detectRadius = 200f;        
    public float loseSightRadius = 400f;      
    public float viewAngle = 360f;          
    public LayerMask obstacleMask;          

    [Header("Attack")]
    public float attackRange = 4.0f;       
    public float attackCooldown = 1.2f;      
    public int   attackDamage = 10;
    private float _lastAttackTime = -999f;

    [Header("Patrol")]
    public Transform[] waypoints;            
    public float waypointReachThreshold = 0.5f;
    private int _wpIndex = 0;

    [Header("Alert Icon")]
    public GameObject alert;

    private RespawnOnPlaneLanding playerRespawn;
    void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {
        agent = agent ?? GetComponent<NavMeshAgent>();   

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
        if (player != null)
            playerRespawn = player.GetComponent<RespawnOnPlaneLanding>();
    }

    void Start()
    {
        if (state == State.Patrol) SetNextWaypointAsDestination();
    }

    void Update()
    {
        if (!player || !agent) return;

        float dist = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Patrol:
                if (PlayerVisible() && dist <= detectRadius) SwitchState(State.Chase);
                PatrolTick();
                break;

            case State.Chase:
                agent.stoppingDistance = attackRange;
                agent.SetDestination(player.position);

                if (dist <= attackRange) SwitchState(State.Attack);
                else if (dist > loseSightRadius && !PlayerVisible())
                    SwitchState(State.Patrol);
                break;

            case State.Attack:
                agent.isStopped = true;
                Face(player.position);

                if (Time.time - _lastAttackTime >= attackCooldown)
                {
                    _lastAttackTime = Time.time;
                    Debug.Log("Enemy attacked!");
                    StartCoroutine(playerRespawn.RespawnRoutine());
                
                }

                if (dist > attackRange * 1.1f)
                {
                    agent.isStopped = false;
                    SwitchState(State.Chase);
                }
                break;
        }
    }

    void SwitchState(State next)
    {
        state = next;
        if (state == State.Patrol)
        {
            alert.SetActive(false);
            agent.stoppingDistance = 0f;
            agent.isStopped = false;
            SetNextWaypointAsDestination();
        }
        else if (state == State.Chase)
        {
            alert.SetActive(true);
            agent.isStopped = false;
        }
        else if (state == State.Attack)
        {
            alert.SetActive(true);
            agent.isStopped = true;
        }
    }

    void PatrolTick()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (!agent.hasPath || agent.remainingDistance <= waypointReachThreshold)
            SetNextWaypointAsDestination();
    }

    void SetNextWaypointAsDestination()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        agent.SetDestination(waypoints[_wpIndex].position);
        _wpIndex = (_wpIndex + 1) % waypoints.Length;
    }

    bool PlayerVisible()
    {
        Vector3 dir = (player.position - transform.position);
        Vector3 flatDir = new Vector3(dir.x, 0, dir.z);
        if (flatDir.magnitude > detectRadius) return false;

        float angle = Vector3.Angle(transform.forward, flatDir.normalized);
        if (angle > viewAngle * 0.5f) return false;

        if (obstacleMask.value != 0)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 1.0f,
                                flatDir.normalized, out RaycastHit hit, detectRadius, ~0))
            {
                if (hit.transform != player) return false;
            }
        }
        return true;
    }

    void Face(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion q = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
