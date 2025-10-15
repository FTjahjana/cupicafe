using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player;
    public GameObject hand; public Transform handT; public bool handIsHolding = false;

    public float mapRadius; public Vector3 mapCenter;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("Game started!");
    }

    public void Start()
    {
        handT = hand.transform; 
    }

    public void EndGame()
    {
        Debug.Log("Game ended!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(mapCenter, mapRadius);
    }
}
