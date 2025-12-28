using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AgentMover : MonoBehaviour
{
    public int NPC_ID;

    [Header("Waypoint Stuff")]
    //public List<Transform> waypoints;
    //private int currentIndex;
    public Transform currentDestination;
    public float offsetDistance;

    public bool Customer = true;

    public Transform waitOutsideSpot;
    public Collider welcmatCol;

    [Header("Agent Stuff")]
    public float agentSpeed;

    public NavMeshAgent agent;
    public bool affectRotation = true;

    [Header("Queue Stuff")]
    public NPCQueue npcQueue;

    public bool arrivedAtQueue = false;
    public bool isWaitingOutside = false;
    [Space(5)]
    public bool walkingtoPS, reachedPS; [HideInInspector] public bool PS_AorB;
    public NPCQueue.PairSlot assignedPairSlot;
    [Space(5)]
    public bool reachedRS, walkingToRS;
    public NPCQueue.RandomSlot assignedRandomSlot;
    [Space(5)]
    public CharAnimations anims;

    void Start()
    {
        Debug.Log("Hi im a new NPC. My NPC ID is "+ NPC_ID);
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) 
        {Debug.LogError("navmeshagent missing");enabled = false; return;}

        anims = GetComponent<CharAnimations>();

        /*if (waypoints == null || waypoints.Count == 0)
        { Debug.LogError("waypoints empty");  return;}
        
        currentIndex = 0;
        currentDestination = waypoints[currentIndex]*/ ;

        if (Customer) currentDestination = waitOutsideSpot;

        agent.destination = currentDestination.position;
        agent.speed = agentSpeed;
    }

    void Update()
    {
        //if (waypoints == null || waypoints.Count == 0)
        //    return;

        if (currentDestination == null)
            return;
        
        if (Vector3.Distance(transform.position, agent.destination) < offsetDistance)
        {
            if (!Customer) return;
            if(!arrivedAtQueue){for (int i = 0; i < npcQueue.CustomerQueue.Count; i++)
            {
                if (currentDestination == npcQueue.CustomerQueue[i].slotLoc)
                { npcQueue.UpdateQueue(); arrivedAtQueue = true; 
                return;}
            }}

            if (walkingToRS && !reachedRS)
            {
                reachedRS = true;
                walkingToRS = false;
            }

            if (walkingtoPS && !reachedPS)
            {
                reachedPS = true; walkingtoPS = false;

                agent.enabled = false; 

                Transform slotT = PS_AorB ? assignedPairSlot.slotB : assignedPairSlot.slotA;

                transform.SetParent(slotT);
                transform.localPosition = Vector3.zero; transform.localRotation = Quaternion.identity;

                anims.CallAnimByTrigger(PS_AorB ? "SitB" : "SitA");
            }
        }

        if (affectRotation)
        {agent.updateRotation = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, currentDestination.rotation,
            Time.deltaTime * agent.angularSpeed
        );}

    }

     void OnTriggerEnter(Collider other)
    {
        if (!Customer) return;
        if (other == welcmatCol && currentDestination == waitOutsideSpot && !isWaitingOutside)
        {
            npcQueue.waitingAgents.Add(this.gameObject); 
            isWaitingOutside = true;
            Debug.Log("NPC " + NPC_ID + " is waiting outside.");

            npcQueue.UpdateQueue();
            GetComponent<Collider>().isTrigger = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, offsetDistance);
    }
}