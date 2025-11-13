using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AgentMover : MonoBehaviour
{
    public int NPC_ID;

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
        Debug.Log("Hi im a new NPC. My NPC ID is "+ NPC_ID);
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) 
        {Debug.LogError("navmeshagent missing");enabled = false; return;}

        if (waypoints == null || waypoints.Count == 0)
        { Debug.LogError("waypoints empty");  return;}
        
        currentIndex = 0;
        currentDestination = waypoints[currentIndex];
        agent.destination = currentDestination.position;
        agent.speed = agentSpeed;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        if (currentDestination == null)
            return;
        
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