using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance { get; private set; }
    public Transform hand;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        hand = this.gameObject.transform;
        DontDestroyOnLoad(gameObject);
    }
}
