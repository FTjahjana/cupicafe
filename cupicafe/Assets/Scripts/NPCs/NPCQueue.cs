using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCQueue : MonoBehaviour
{

    [System.Serializable]
    public class CustomerQueueSlot
    {
        public Transform slotLoc; public GameObject occupier;
    }

    [System.Serializable]
    public class RandomSlot
    {
        public Transform slotLoc;
        public GameObject occupier;
    }

    [System.Serializable]
    public class PairSlot
    {
        public Transform slotA;
        public Transform slotB;
        public GameObject occupierA;
        public GameObject occupierB;

        public bool IsFree => occupierA == null && occupierB == null;
    }

    [System.Serializable]
    public struct DecayRange
    {
        public float min;public float max;
        public float Random => UnityEngine.Random.Range(min, max);
    }

    public NPCSpawner spawner;
    public List<GameObject> waitingAgents;
    [SerializeField] public List<CustomerQueueSlot> CustomerQueue; //slots
    public Door door;
    [SerializeField] public List<RandomSlot> randomSlots;
    [SerializeField] public List<PairSlot> pairSlots;

    [Header("Decay / Exit Rates")]
    public DecayRange Q2RS_Delay;   
    public DecayRange RS_Destroy;    
    public DecayRange PS_Destroy;   

    [ContextMenu("TEST / Dequeue First")]
    void Test_DequeueFirst()
    {
        DequeueCustomer(CustomerQueue[0].occupier);
    }

    [ContextMenu("TEST / 0(CQ) → RS")]
    void FirstInQueueToRandom()
    {
        Q2RS(CustomerQueue[0].occupier);
    }

    [ContextMenu("TEST / 0,1(CQ)-> PS")]
    void PairFirstTwoINQueue()
    {
        if (CustomerQueue.Count < 2) return;
        if (CustomerQueue[0].occupier == null || CustomerQueue[1].occupier == null) return;

        TryAssignPair(CustomerQueue[0].occupier, CustomerQueue[1].occupier);
    }


    public IEnumerator QueueActive(bool airplaneticket) // bool name was random
    // call this in SOE or something
    {
        if (airplaneticket)
        {
            InvokeRepeating(nameof(UpdateQueue), 0f, 3f);

            // Q2RS
            StartCoroutine(Q2RSLoop());

            // Destruction
            StartCoroutine(PSDestroyCycle());
            StartCoroutine(RSDestroyCycle());
            
            yield break;
        }
    else
        {
            CancelInvoke(nameof(UpdateQueue));
            StopAllCoroutines();

            foreach (var slot in randomSlots) { if (slot.occupier != null) SetObstacle(slot.slotLoc, false); }
            foreach (var p in pairSlots) { if (p.occupierA != null) SetObstacle(p.slotA, false); if (p.occupierB != null) SetObstacle(p.slotB, false); }
        
            yield break;
        }
    }

    public void UpdateQueue()
    {
        bool assigned = false;

        for (int i = 0; i < CustomerQueue.Count; i++)
        {
            if (CustomerQueue[i].occupier == null)
            {
                if (i > 0) // if ur not first guy
                {
                    var prev = CustomerQueue[i - 1].occupier;
                    if (prev != null && !prev.GetComponent<AgentMover>().arrivedAtQueue)
                        return; // previous hasn't arrived yet
                }

                if (waitingAgents.Count == 0) return; // is anyone waiting

                GameObject newCustomer = waitingAgents[0];
                waitingAgents.RemoveAt(0); // i stole the first guy
                CustomerQueue[i].occupier = newCustomer; // youre a customer now
                SetObstacle(CustomerQueue[i].slotLoc, true); 
                if (!door.isOpen) door.OpenDoor();

                AgentMover newCustomerMover = newCustomer.GetComponent<AgentMover>();
                newCustomerMover.currentDestination 
                = CustomerQueue[i].slotLoc; //replace current destination
                newCustomerMover.agent.destination 
                = CustomerQueue[i].slotLoc.position; // same thing but the actual destination

                newCustomerMover.isWaitingOutside = false;
                
                assigned = true;
                break;
            }
        }

         if (!assigned && waitingAgents.Count > 0)
        {
            Debug.Log("Customer Queue is now full. Customers waiting outside: " + waitingAgents.Count);
            return;
        }
    }

    public void DequeueCustomer(GameObject customer)
    {
        if (CustomerQueue.Count == 0) return;
    for (int i = 0; i < CustomerQueue.Count; i++)
    {
        if (CustomerQueue[i].occupier == customer) // is this you?
        { 
            //Debug.Log (customer.name + " : Bye !");
            //customer.GetComponent<CharAnimations>().CallAnimByTrigger("Bye");
            CustomerQueue[i].occupier = null; // bye
            SetObstacle(CustomerQueue[i].slotLoc, false); 

            for (int j = i + 1; j < CustomerQueue.Count; j++) // everyone move alongQ
            {
                if (CustomerQueue[j].occupier != null && CustomerQueue[j - 1].occupier == null)
                {
                    var mover = CustomerQueue[j].occupier.GetComponent<AgentMover>();
                    mover.currentDestination = CustomerQueue[j - 1].slotLoc;
                    mover.agent.destination = CustomerQueue[j - 1].slotLoc.position;
                    CustomerQueue[j].occupier = null;
                }
            }
            break;
        }
    }
    }

    public void ReleaseFromRS(GameObject customer)
    {
        foreach (var slot in randomSlots)
        { if (slot.occupier == customer)
        { slot.occupier = null; 
        SetObstacle(slot.slotLoc, false);
         break; } }
    }
    public void ReleaseFromPS(GameObject customer)
    {
        foreach (var p in pairSlots)
        {
            if (p.occupierA == customer) { p.occupierA = null; SetObstacle(p.slotA, false); }
            if (p.occupierB == customer) { p.occupierB = null; SetObstacle(p.slotB, false); } 

        }
    }
    public void RemoveFromAllSlots(GameObject customer)
    {
        DequeueCustomer(customer);
        ReleaseFromRS(customer);
        ReleaseFromPS(customer);
    }

    (GameObject customer, RandomSlot slot) Q2RS(GameObject customer)
    {
        if (CustomerQueue.Count == 0)
            return (null, null);

        DequeueCustomer(customer);

        var slot = GetRS();
        if (slot == null)
            return (customer, null);

        slot.occupier = customer;
        SetObstacle(slot.slotLoc, true);

        var agentM = customer.GetComponent<AgentMover>();
        agentM.currentDestination = slot.slotLoc;
        agentM.agent.destination = slot.slotLoc.position;

        Debug.Log($"<color=green>Q2RS: </color>{customer.name} → {slot.slotLoc.name}");
        return (customer, slot);
    }
    IEnumerator Q2RSLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Q2RS_Delay.Random);
            Q2RS(CustomerQueue[0].occupier);
        }
    }


    RandomSlot GetRS()
    {
        var free = randomSlots.FindAll(slot => slot.occupier == null);
        if (free.Count == 0) return null;
        return free[Random.Range(0, free.Count)];
    }

    IEnumerator RSDestroyCycle()
    {
        while (true)
        {
            var occupied = randomSlots.FindAll(r => r.occupier != null &&
                r.occupier.GetComponent<AgentMover>()?.reachedRS == true);

            if (occupied.Count <2) {yield return new WaitForSeconds(4f);continue;}
            
            var slot = occupied[Random.Range(0, occupied.Count)];
            yield return new WaitForSeconds(RS_Destroy.Random);

            Debug.Log($"<color=red>RSDestroy: </color>{slot.slotLoc.name}, {slot.occupier.name}");
            Debug.Log (slot.occupier.name + " : Bye !");
            slot.occupier.GetComponent<CharAnimations>().CallAnimByTrigger("Bye");

            slot.occupier = null; SetObstacle(slot.slotLoc, false);
        }
    }

    public bool TryAssignPair(GameObject a, GameObject b)
    {
        var slot = pairSlots.Find(pairslot => pairslot.IsFree);
        if (slot == null) return false;

        RemoveFromAllSlots(a); RemoveFromAllSlots(b);
        slot.occupierA = a; slot.occupierB = b;
        SetObstacle(slot.slotA, true); SetObstacle(slot.slotB, true); 

        Debug.Log($"<color=pink>TryAssignPair: </color>{a.name}, {b.name} → {slot.slotA.parent.name}");

        var ma = a.GetComponent<AgentMover>(); var mb = b.GetComponent<AgentMover>();

        ma.offsetDistance = ma.offsetDistance/2;
        ma.currentDestination = slot.slotA; ma.agent.destination = slot.slotA.position;
        mb.offsetDistance = mb.offsetDistance/2;
        mb.currentDestination = slot.slotB; mb.agent.destination = slot.slotB.position;

        ma.assignedPairSlot = slot; mb.assignedPairSlot = slot;
        ma.PS_AorB = false; mb.PS_AorB = true;
        ma.walkingtoPS = true; mb.walkingtoPS = true;
        
        return true;
    }

    IEnumerator PSDestroyCycle()
    {
        while (true)
        {
            var occupied = pairSlots.FindAll(p =>
            p.occupierA != null && p.occupierB != null &&
            p.occupierA.GetComponent<AgentMover>()?.reachedPS == true &&
            p.occupierB.GetComponent<AgentMover>()?.reachedPS == true
            );

            if (occupied.Count <2) {yield return new WaitForSeconds(4f);continue;}

            float t = occupied.Count >= pairSlots.Count - 1
                ? PS_Destroy.Random/2 /*almost full → faster*/ : PS_Destroy.Random; // default

            var slot = occupied[Random.Range(0, occupied.Count)];

            yield return new WaitForSeconds(t);

            Debug.Log($"<color=red>PSDestroy: </color>{slot.slotA.parent.name}, {slot.occupierA.name}&{slot.occupierB.name}");
            slot.occupierA.GetComponent<CharAnimations>()?.CallAnimByTrigger("Bye");
            slot.occupierB.GetComponent<CharAnimations>()?.CallAnimByTrigger("Bye");

            slot.occupierA = null;
            slot.occupierB = null;

            SetObstacle(slot.slotA, false);
            SetObstacle(slot.slotB, false);
        }
    }

    void SetObstacle(Transform slotTransform, bool active)
    {
        var obstacle = slotTransform.GetComponent<NavMeshObstacle>();
        if (obstacle != null) obstacle.enabled = active;
    }

    

    



    

}