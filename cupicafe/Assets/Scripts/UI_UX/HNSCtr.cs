using UnityEngine;

public class HNSCtr : MonoBehaviour
{
    public Hearts hearts;
    
    public void SetupNewScore()
    {
        if (hearts.heart1target != null && hearts.heart2target != null) hearts.NewScore();
    }
}