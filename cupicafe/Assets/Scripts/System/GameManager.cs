using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int SOEindex = 0; public SOE SOE;

    public GameObject Player;
    public Transform hand; public bool handIsHolding = false;

    public float mapRadius; public Vector3 mapCenter;

    public bool inGame;
    

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

    void OnEnable()
    { SceneManager.sceneLoaded += OnSceneLoaded; }

    void OnDisable()
    { SceneManager.sceneLoaded -= OnSceneLoaded; }

    public void EndGame()
    {
        Debug.Log("Game ended!");
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            inGame = true;
            Player = GameObject.FindGameObjectWithTag("Player");
            hand = GameObject.FindGameObjectWithTag("Hand").transform;

            SOE = GameObject.FindGameObjectWithTag("SOE").GetComponent<SOE>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mapCenter, mapRadius);
    }
    
    public void IncSOE(){ 
        if (SOE != null) {SOEindex++; SOE.NewSOE();}
        else Debug.Log("SOE Unassigned");
     }
}
