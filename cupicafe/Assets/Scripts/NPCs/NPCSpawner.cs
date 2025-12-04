using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;

    public int NPCCounter;

    public List<Transform> waypoints;
    
    public float startDelay;

    public int numberOfNPCs = 10;
    public float spawnRadius = 5f;

    [System.Serializable]
    public struct spawnRound
    {
        public int amount;
        public float rate;

        public float timeUntilNextRound;
    }
    
    public List<spawnRound> spawnRounds;

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
        Vector3 randomPoint = transform.position + Random.insideUnitSphere //just found this.. cool
        * spawnRadius;
        randomPoint.y = transform.position.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            Vector3 spawnPosition = hit.position;

            GameObject newNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

            AgentMover agentMover = newNPC.GetComponent<AgentMover>();
            if (agentMover != null && waypoints.Count > 0)
            {
                agentMover.waypoints = new List<Transform>(waypoints);
                agentMover.NPC_ID = NPCCounter;
                NPCCounter++;
            }
            else
            {
                Debug.LogWarning("uhh somethings missing either agentMover script or the target.");
            }
        }
        else
        {
            Debug.LogWarning("no space to spawn an npc");
        }
    }

    public void SpawnPopupNPC(Vector3 spawnPointPos, float spawnPointYRot)
    {
        Instantiate(npcPrefab, spawnPointPos, Quaternion.Euler(0, spawnPointYRot, 0));
    }
    
    IEnumerator SpawnNPCsCoroutine(int amount, float rate)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnNPC();
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator SpawnAllRounds()
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