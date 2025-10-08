using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup animationPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeRoutine;

    private void Start()
    {
        animationPanel.alpha = 0f;
        animationPanel.gameObject.SetActive(false);
    }

    public void PlayAnimation(string animTrigger)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        animationPanel.gameObject.SetActive(true);
        fadeRoutine = StartCoroutine(FadeCanvas(animationPanel, 0f, 1f, fadeDuration, () =>
        {
            animator.SetTrigger(animTrigger);
        }));
    }

    public void AnimFadeOut()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeCanvas(animationPanel, 1f, 0f, fadeDuration, () =>
        {
            animationPanel.gameObject.SetActive(false);
        }));
    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, float start, float end, float duration, System.Action onComplete)
    {
        float elapsed = 0f;
        canvas.alpha = start;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        canvas.alpha = end;
        onComplete?.Invoke();
    }
}
