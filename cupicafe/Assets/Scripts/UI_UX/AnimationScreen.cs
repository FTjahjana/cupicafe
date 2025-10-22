using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup animationPanel;
    [SerializeField] private bool fadeIn = false, fadeOut = false;


    public void ShowScreen() { fadeIn = true; }
    public void HideScreen() { fadeOut = true; }
    private void Update()
    {
        if (fadeIn)
        {
            if (animationPanel.alpha < 1)
            {
                animationPanel.alpha += Time.deltaTime;
                if (animationPanel.alpha >= 1) fadeIn = false;
            }
        }

        if (fadeIn)
        {
            if (animationPanel.alpha >= 0)
            {
                animationPanel.alpha += Time.deltaTime;
                if (animationPanel.alpha == 1) fadeOut = false;
            }
        }
    }

    
}
