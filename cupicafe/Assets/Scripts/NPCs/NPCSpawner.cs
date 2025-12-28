using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public GameObject[] specialNpcVariants;

    public int NPCCounter;

    //public List<Transform> waypoints;
    public NPCQueue npcQueue;
    public Transform waitOutsideSpot;
    public Collider welcmatCol;
    
    public float startDelay;

    public int numberOfNPCs = 0;
    public float spawnRadius = 5f;

    [System.Serializable]
    public struct spawnRound
    {
        public int amount;
        public float rate;

        public float timeUntilNextRound;
    }
    
    public List<spawnRound> spawnRounds;
    public Hearts hearts;

    void Start()
    {
        //EventManager.OnQuestStarted += (quest) => StartCoroutine(SpawnAllRounds());
        //StartCoroutine(SpawnAllRounds());
    }

    void Update()
    {

    }


    void SpawnNPC()
    {
        if (numberOfNPCs > 14) 
        {Debug.LogWarning("Too many NPCs are in the scene right now, try spawn again later."); 
        return;}
        if (npcQueue.waitingAgents.Count > 3) 
        {Debug.LogWarning("Too many NPCs waiting outside door. Let them in before trying to spawn more."); 
        return;}
        Vector3 randomPoint = transform.position + Random.insideUnitSphere //just found this.. cool
        * spawnRadius;
        randomPoint.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            Vector3 spawnPosition = hit.position;

            GameObject newNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            
            var N_int = newNPC.GetComponent<NPCInteract>();
            if (N_int != null){ N_int.spawner = this; N_int.hearts = hearts; }
            
            AgentMover agentMover = newNPC.GetComponent<AgentMover>();
            if (agentMover != null /*&& waypoints.Count > 0*/)
            {
                //agentMover.waypoints = new List<Transform>(waypoints);
                agentMover.NPC_ID = NPCCounter;
                newNPC.name = "NPC" + agentMover.NPC_ID;
                NPCCounter++;

                agentMover.npcQueue = npcQueue;
                agentMover.waitOutsideSpot = waitOutsideSpot;
                agentMover.welcmatCol = welcmatCol;
            }
            else
            {
                Debug.LogWarning("uhh somethings missing either agentMover script or the target.");
            }

            numberOfNPCs++;
            
        }
        else
        {
            Debug.LogWarning("no space to spawn an npc");
        }
    }

    public void SpawnPopupNPC(Vector3 spawnPointPos, float spawnPointYRot)
    {
        GameObject newNPC = Instantiate(npcPrefab, spawnPointPos, Quaternion.Euler(0, spawnPointYRot, 0));
        
        newNPC.GetComponent<NavMeshAgent>().enabled = false;
        newNPC.GetComponent<AgentMover>().enabled = false;
        newNPC.GetComponent<NPCInteract>().spawner = this;
        newNPC.GetComponent<NPCInteract>().hearts = hearts;

        Rigidbody rb = newNPC.GetComponent<Rigidbody>(); rb.useGravity = true; rb.isKinematic = false;
    }

    public GameObject SpawnPopupNPC(string name, Vector3 spawnPointPos, float spawnPointYRot)
    {
        foreach (var v in specialNpcVariants) {
        if (v.name == name) {
            GameObject newNPC = Instantiate(v, spawnPointPos, Quaternion.Euler(0, spawnPointYRot, 0));
            newNPC.GetComponent<NavMeshAgent>().enabled = false;
            newNPC.GetComponent<AgentMover>().enabled = false;
            newNPC.GetComponent<NPCInteract>().hearts = hearts;

            Rigidbody rb = newNPC.GetComponent<Rigidbody>(); rb.useGravity = true; rb.isKinematic = false;

            return newNPC;
        } }

        Debug.LogError("Trying to spawn nonexistent special npc prefab variant!");
        return null;
    }
    
    IEnumerator SpawnNPCsCoroutine(int amount, float rate)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnNPC();
            yield return new WaitForSeconds(rate);
        }
    }

    public IEnumerator SpawnAllRounds()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (var n in spawnRounds)
        {
            yield return StartCoroutine(SpawnNPCsCoroutine(n.amount, n.rate));
            yield return new WaitForSeconds(n.timeUntilNextRound);
        }
    }
    
    public void SpawnRound(int i)
    {
        StartCoroutine(SpawnNPCsCoroutine(spawnRounds[i].amount, spawnRounds[i].rate));
    }

}