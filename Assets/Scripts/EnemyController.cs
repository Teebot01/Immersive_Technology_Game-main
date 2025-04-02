using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class EnemyController : MonoBehaviour
{
    private enum AIState
    {
        Idle, Patrolling, Chasing, Stunned
    }

    [Header("Patrol")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float waitAtPoint = 2f;
    private int currentWaypoint;
    private float waitCounter;

    [Header("Components")]
    private NavMeshAgent agent;

    [Header("AI States")]
    [SerializeField] private AIState currentState = AIState.Idle;

    [Header("Chasing")]
    [SerializeField] private float chaseRange;

    [Header("Suspicious")]
    [SerializeField] private float suspiciousTime;
    private float timeSinceLastSawPlayer;

    [Header("Stunned")]
    public float stopDuration = 3f;
    private bool isStopped = false;
    private bool canBeStunned = true;   // Prevents multiple stuns

    private GameObject player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found!");
            return;
        }

        if (wayPoints == null || wayPoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to EnemyController on " + gameObject.name);
            return;
        }

        waitCounter = waitAtPoint;
        timeSinceLastSawPlayer = suspiciousTime;
    }

    private void Update()
    {
        // ✅ Prevent the AI from moving or switching states if stunned
        if (currentState == AIState.Stunned)
        {
            return;
        }

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState(distanceToPlayer);
                break;

            case AIState.Patrolling:
                HandlePatrollingState(distanceToPlayer);
                break;

            case AIState.Chasing:
                HandleChasingState(distanceToPlayer);
                break;
        }
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
        }
        else
        {
            currentState = AIState.Patrolling;
            agent.isStopped = false;
            agent.SetDestination(wayPoints[currentWaypoint].position);
        }

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.Chasing;
        }
    }

    private void HandlePatrollingState(float distanceToPlayer)
    {
        if (agent.remainingDistance <= 0.2f && !agent.pathPending)
        {
            currentWaypoint++;
            if (currentWaypoint >= wayPoints.Length)
            {
                currentWaypoint = 0;
            }
            currentState = AIState.Idle;
            waitCounter = waitAtPoint;
        }

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.Chasing;
        }
    }

    private void HandleChasingState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);

        if (distanceToPlayer > chaseRange)
        {
            ChangeSpeed(2f);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            timeSinceLastSawPlayer -= Time.deltaTime;

            if (timeSinceLastSawPlayer <= 0)
            {
                currentState = AIState.Patrolling;
                timeSinceLastSawPlayer = suspiciousTime;
                agent.isStopped = false;
                agent.SetDestination(wayPoints[currentWaypoint].position);
            }
        }
    }

    public void ChangeSpeed(float newSpeed)
    {
        if (agent != null)
        {
            agent.speed = newSpeed;
            Debug.Log("Agent speed changed to: " + newSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only stun the enemy if it can be stunned
        if (collision.gameObject.CompareTag("Flashlight") && canBeStunned)
        {
            StartCoroutine(StopAgentTemporarily());
        }
    }

    private IEnumerator StopAgentTemporarily()
    {
        isStopped = true;
        canBeStunned = false;         // Prevent repeated stuns
        currentState = AIState.Stunned;

        // Stop movement and rotation
        agent.isStopped = true;
        agent.updateRotation = false;

        yield return new WaitForSeconds(stopDuration);

        // Resume movement and rotation
        agent.isStopped = false;
        agent.updateRotation = true;
        isStopped = false;

        // ✅ Return to previous state (patrolling) only AFTER stun ends
        currentState = AIState.Patrolling;
        agent.SetDestination(wayPoints[currentWaypoint].position);

        // Reactivate stun capability after stun ends
        canBeStunned = true;
    }
}

