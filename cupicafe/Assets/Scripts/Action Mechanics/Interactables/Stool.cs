using UnityEngine;
using System.Collections;

public class Stool : MonoBehaviour, IInteractable
{
    private bool isOut = true;
    public Animator anim; bool canClick = true;
    [Tooltip("(Convert to decimal value before input)")][SerializeField] float animLength;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Interact()
    {
        if (!canClick) return;
        if (!isOut) GoOut(); else GoIn();

        isOut = !isOut;

        StartCoroutine(Unlock());
    }

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(animLength);
        canClick = true;                       
    }

    void GoOut()
    {
        Debug.Log("Stool dragged out.");
        anim.SetTrigger("Out");

        QuestTrigger qt = GetComponent<QuestTrigger>();
        if (qt != null && qt.enabled) qt.TriggerCondition();
    }

    void GoIn()
    {
        Debug.Log("Stool placed back in.");
        anim.SetTrigger("In");
    }


}
