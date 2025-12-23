using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))] 
public class ControlledTextComponent : MonoBehaviour
{
    private TMP_Text textComponent;
    private RectTransform rectTransform;

    [Header("Container Scaling")]
    public bool resizeContainer = false;
    public Vector2 minContainerSize = new Vector2(450, 75);
    public Vector2 maxContainerSize = new Vector2(600, 100);

    [Header("Size Boundaries (Font Size)")]
    public float minFontSize = 12f, maxFontSize = 30f;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        if (TextOptions.Instance != null){TextOptions.Instance.RegisterText(this);}

        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void ApplyScale(float globalScaleValue)
    {
        if (textComponent != null)
        {
            textComponent.fontSize = Mathf.Lerp(minFontSize, maxFontSize, globalScaleValue);
        }
        if (resizeContainer && rectTransform != null)
        {
            rectTransform.sizeDelta = Vector2.Lerp(minContainerSize, maxContainerSize, globalScaleValue);
        }
    }
}