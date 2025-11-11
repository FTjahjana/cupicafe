using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AgentMover : MonoBehaviour
{
    [Header("Waypoint Stuff")]
    public List<Transform> waypoints;
    private int currentIndex;
    public Transform currentDestination;
    public float offsetDistance;

    [Header("Agent Stuff")]
    public float agentSpeed;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("navmeshagent missing");

        currentIndex = 0;
        currentDestination = waypoints[currentIndex];
        agent.destination = currentDestination.position;
        agent.speed = agentSpeed;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, currentDestination.position) < offsetDistance)
        {
            currentIndex++;
            if (currentIndex == waypoints.Count)
            {
                currentIndex = 0;
            }
            currentDestination = waypoints[currentIndex];
            agent.destination = currentDestination.position;

        }
    }
}