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

    public List<GameObject> waitingAgents;
    [SerializeField] public List<CustomerQueueSlot> CustomerQueue; //slots
    public Door door;
    [SerializeField] public List<RandomSlot> randomSlots;
    [SerializeField] public List<PairSlot> pairSlots;

    [ContextMenu("TEST / Dequeue First")]
    void Test_DequeueFirst()
    {
        if (CustomerQueue.Count == 0) return;
        if (CustomerQueue[0].occupier == null) return;

        DequeueCustomer(CustomerQueue[0].occupier);
    }

    [ContextMenu("TEST / First (CQ) → RS")]
    void Test_FirstToRandom()
    {
        if (CustomerQueue[0].occupier == null) return;

        var c = CustomerQueue[0].occupier; DequeueCustomer(c);

        var slot = GetRS(); if (slot == null) return;

        slot.occupier = c;
        SetObstacle(slot.slotLoc, true); //new need test

        var m = c.GetComponent<AgentMover>();
        m.currentDestination = slot.slotLoc;
        m.agent.destination = slot.slotLoc.position;
    }

    [ContextMenu("TEST / First Two (CQ)-> PS")]
    void Test_PairFirstTwo()
    {
        if (CustomerQueue.Count < 2) return;
        if (CustomerQueue[0].occupier == null) return;
        if (CustomerQueue[1].occupier == null) return;

        TryAssignPair(
            CustomerQueue[0].occupier,
            CustomerQueue[1].occupier
        );
    }


    public void QueueActive(bool airplaneticket) // bool name was random
    // call this in SOE or something
    {
        if (airplaneticket)
        {
            InvokeRepeating(nameof(UpdateQueue), 0f, 3f);
            StartCoroutine(Q2RS());
        }
    else
        {
            CancelInvoke(nameof(UpdateQueue));
            StopAllCoroutines();

            foreach (var slot in randomSlots) { if (slot.occupier != null) SetObstacle(slot.slotLoc, false); }
            foreach (var p in pairSlots) { if (p.occupierA != null) SetObstacle(p.slotA, false); if (p.occupierB != null) SetObstacle(p.slotB, false); }
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
    for (int i = 0; i < CustomerQueue.Count; i++)
    {
        if (CustomerQueue[i].occupier == customer) // is this you?
        { 
            //Debug.Log (customer.name + " : Bye !");
            //customer.GetComponent<CharAnimations>().CallAnim("Bye");
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
        SetObstacle(slot.slotLoc, false);//new need test
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

    IEnumerator Q2RS()
    {
        yield return new WaitForSeconds(Random.Range(4f, 8f));

        if (CustomerQueue.Count == 0) yield break;
        var first = CustomerQueue[0].occupier;
        if (first == null) yield break;

        DequeueCustomer(first);

        RandomSlot slot = GetRS();
        if (slot == null) yield break;

        slot.occupier = first;
        SetObstacle(slot.slotLoc, true);

        var mover = first.GetComponent<AgentMover>();
        mover.currentDestination = slot.slotLoc;
        mover.agent.destination = slot.slotLoc.position;

        StartCoroutine(RSCycle(first, slot));
    }

    RandomSlot GetRS()
    {
        var free = randomSlots.FindAll(slot => slot.occupier == null);
        if (free.Count == 0) return null;
        return free[Random.Range(0, free.Count)];
    }

    IEnumerator RSCycle(GameObject customer, RandomSlot slot)
    {
        yield return new WaitForSeconds(Random.Range(8f, 12f));
        if (slot.occupier != customer) yield break;
        Debug.Log (customer.name + " : Bye !");
        customer.GetComponent<CharAnimations>().CallAnim("Bye");

        slot.occupier = null; SetObstacle(slot.slotLoc, false);
    }

    public bool TryAssignPair(GameObject a, GameObject b)
    {
        var slot = pairSlots.Find(pairslot => pairslot.IsFree);
        if (slot == null) return false;

        RemoveFromAllSlots(a); RemoveFromAllSlots(b);
        slot.occupierA = a; slot.occupierB = b;
        SetObstacle(slot.slotA, true); SetObstacle(slot.slotB, true); 

        var ma = a.GetComponent<AgentMover>(); var mb = b.GetComponent<AgentMover>();

        ma.currentDestination = slot.slotA; ma.agent.destination = slot.slotA.position;
        mb.currentDestination = slot.slotB; mb.agent.destination = slot.slotB.position;

        return true;
    }

    void SetObstacle(Transform slotTransform, bool active)
    {
        var obstacle = slotTransform.GetComponent<NavMeshObstacle>();
        if (obstacle != null) obstacle.enabled = active;
    }

    

    



    

}