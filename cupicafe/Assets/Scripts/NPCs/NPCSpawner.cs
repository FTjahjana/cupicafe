using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    
    public Transform target;
    
    public int numberOfNPCs = 10;
    public float spawnRadius = 5f;

    void Start()
    {

    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnNPCs();
        }

    }

    void SpawnNPCs()
    {
        for (int i = 0; i < numberOfNPCs; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere //just found this.. cool
            * spawnRadius;
            randomPoint.y = transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
            //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AI.NavMesh.SamplePosition.html
            { Vector3 spawnPosition = hit.position;
                
                GameObject newNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
                
                AgentMover agentMover = newNPC.GetComponent<AgentMover>();
                if (agentMover != null && target != null)
                {
                    agentMover.target = target;
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
    }
}