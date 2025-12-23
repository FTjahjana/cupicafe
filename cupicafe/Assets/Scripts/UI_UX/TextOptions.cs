using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOptions : MonoBehaviour
{
    public static TextOptions Instance { get; private set; }
    public Slider slider;
    private List<ControlledTextComponent> registeredTexts = new List<ControlledTextComponent>();

    private float currentScaleValue = 0.5f; 

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

    public void RegisterText(ControlledTextComponent textComponent)
    {
        if (!registeredTexts.Contains(textComponent))
        {
            registeredTexts.Add(textComponent);
            textComponent.ApplyScale(currentScaleValue);
        }
    }

    public void SetGlobalScaleFromSlider()
    {
        if (slider != null)
            SetGlobalScale(slider.value);
    }
    public void SetGlobalScale(float value) 
    {
        currentScaleValue = Mathf.Clamp01(value); 

        foreach (ControlledTextComponent text in registeredTexts)
        {if (text != null){ text.ApplyScale(currentScaleValue);}
        }
    }
}