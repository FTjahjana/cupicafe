using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    public Animator anim; bool canClick = true;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Interact()
    {
        if (!canClick) return;
        var state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime < 1f) return; // if an anim is still playing

        if (!isOpen) OpenDoor(); else CloseDoor();

        isOpen = !isOpen;
    }

    void OpenDoor()
    {
        Debug.Log("Door opened.");
        anim.SetTrigger("Open");

        QuestTrigger qt = GetComponent<QuestTrigger>();
        if (qt != null && qt.enabled) qt.TriggerCondition();
    }

    void CloseDoor()
    {
        Debug.Log("Door closed.");
        anim.SetTrigger("Close");
    }


}
