using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointerWand : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject pointerPopupText;
    public bool attached; 

    void Start(){PopupText(false,"");}

    public void AttachToChara(GameObject chara)
    {
        attached = true; Debug.Log("<color=#E043A7>POINTER ATTACH → oneTheNpc</color>");
        gameObject.SetActive(true); 
        transform.SetParent(chara.transform, false); 

        anim.Rebind();   
        anim.Update(0f);
    }

    public void Detach()
    {
        attached = false; Debug.Log("<color=#E043A7>POINTER DETACH</color>");
        transform.SetParent(null);
        gameObject.SetActive(false); 
    }

    public void PopupText(bool on, string text = "Pointer Wand Popup Text")
    {
        pointerPopupText.SetActive(on);
        pointerPopupText.GetComponent<TMP_Text>().text = text;
    }
}
