using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 

public class OptionsReferences : MonoBehaviour
{
    private Slider slider; private Volume globalVolume;

    void Start()
    {
        slider = GetComponent<Slider>();
        if (gameObject.name == "Text Size Slider" && TextOptions.Instance != null && slider != null)
        {TextOptions.Instance.slider = slider;}
        if (gameObject.name == "Brightness Slider" && BrightnessOptions.Instance != null && slider != null)
        {BrightnessOptions.Instance.slider = slider;}

        globalVolume = GetComponent<Volume>();
        if (globalVolume != null && BrightnessOptions.Instance != null)
        {BrightnessOptions.Instance.globalVolume = globalVolume;}
        

    } 

}
