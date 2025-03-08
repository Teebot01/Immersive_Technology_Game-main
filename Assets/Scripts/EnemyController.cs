using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private enum AIState
    {
        Idle, Patrolling, Chasing
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

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case AIState.Idle:
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
                break;

            case AIState.Patrolling:
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
                break;

            case AIState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);

                if (distanceToPlayer > chaseRange)
                {
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
                break;
        }
    }
}

