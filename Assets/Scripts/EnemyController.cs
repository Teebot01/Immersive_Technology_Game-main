using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform wayPoints;
    private int currentWaypoint;

    [Header("Components")]
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= 0.2f)
        {
            currentWaypoint++;
            if (currentWaypoint >= wayPoints.childCount)
            {
                currentWaypoint = 0;
            }

            agent.SetDestination(wayPoints.GetChild(currentWaypoint).position);
        }
    }
}
