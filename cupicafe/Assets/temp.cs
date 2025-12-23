using System.Collections;
using UnityEngine;

public class temp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Animator anim;

    public void S2Atrue(){ StartCoroutine(SetS2A(true));}
    public void S2Afalse(){ StartCoroutine(SetS2A(false));}
    private IEnumerator SetS2A(bool thing)
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("S2APopup", thing);
        yield return null;
    }

}
