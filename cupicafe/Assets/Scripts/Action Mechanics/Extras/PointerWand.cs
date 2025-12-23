using UnityEngine;

public class PointerWand : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public bool attached;

    public void AttachToChara(GameObject chara)
    {
        attached = true;
        gameObject.SetActive(true); 
        transform.SetParent(chara.transform, false); 

        anim.Rebind();   
        anim.Update(0f);
    }

    public void Detach()
    {
        attached = false;
        transform.SetParent(null);
        gameObject.SetActive(false); 
    }
}
