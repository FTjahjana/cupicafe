using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 
using UnityEngine.UI;

public class BrightnessOptions : MonoBehaviour
{
    public static BrightnessOptions Instance { get; private set; }
    public Volume globalVolume; public Slider slider;

    [Header("Range")]
    public float minExposure = -1.5f, maxExposure = 3.0f;
    public float minBrightness = 0f, maxBrightness = .5f;

    private ColorAdjustments colorAdjustmentOverride;
    public Image colorFilter;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        if (globalVolume == null)
        {
            Debug.LogError("missing ref to Global Volume.");
            return;
        }

        if (!globalVolume.profile.TryGet(out colorAdjustmentOverride))
        {
            colorAdjustmentOverride = globalVolume.profile.Add<ColorAdjustments>(true);
        }
        colorAdjustmentOverride.active = true;
        colorAdjustmentOverride.postExposure.overrideState = true; 
        SetGlobalBrightness(0.5f);
    }

    public void SetGlobalBrightnessFromSlider()
    {
        if (slider != null)
            SetGlobalBrightness(slider.value);
    }

    public void SetGlobalBrightness(float value)
    {
        float normalizedValue = Mathf.Clamp01(value); 
        float uiBrightness = Mathf.Clamp01(value); 

        if (colorAdjustmentOverride != null)
        {
            float targetExposure = Mathf.Lerp(minExposure, maxExposure, normalizedValue);
            colorAdjustmentOverride.postExposure.value = targetExposure;
        }
        if (colorFilter != null)
        {
            float colorIntensity = Mathf.Lerp( maxBrightness, minBrightness, normalizedValue);
            Color c = colorFilter.color; c.a = colorIntensity;
            colorFilter.color = c;
        }

    }
}