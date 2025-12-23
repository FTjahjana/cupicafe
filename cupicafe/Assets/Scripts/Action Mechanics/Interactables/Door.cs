using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable
{
    public bool isOpen = false;
    public Animator anim; public bool canClick = true;
    public NavMeshObstacle doorObstacle;

    void Start()
    {
        anim = GetComponent<Animator>();
        doorObstacle = GetComponent<NavMeshObstacle>();
    }

    public virtual void Interact()
    {
        if (!canClick) return;
        var state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime < 1f) return; // if an anim is still playing

        if (!isOpen) OpenDoor(); else CloseDoor();
    }

    public void OpenDoor()
    {
        Debug.Log("Door opened.");
        anim.SetTrigger("Open");
        isOpen = true;

        if (doorObstacle!= null)doorObstacle.enabled = false;

        QuestTrigger qt = GetComponent<QuestTrigger>();
        if (qt != null && qt.enabled) qt.TriggerCondition();
    }

    public void CloseDoor()
    {
        Debug.Log("Door closed.");
        anim.SetTrigger("Close");
        isOpen = false;

        if (doorObstacle!= null)doorObstacle.enabled = true;
    }


}
