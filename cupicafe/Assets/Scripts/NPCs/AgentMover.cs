using UnityEngine;
using UnityEngine.AI;

public class AgentMover : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (agent == null) Debug.LogError("navmeshagent missing");
    }

    void Update()
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
        else if (target == null)
        {
            if (agent != null && agent.enabled && agent.hasPath)
            {
                agent.ResetPath();
            }
        }
    }
}