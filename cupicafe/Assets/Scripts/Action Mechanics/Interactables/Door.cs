using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        isOpen = !isOpen;
        if (isOpen) OpenDoor(); else CloseDoor();
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("Door opened.");
            anim.Play("Open");
        }
    }

    void CloseDoor()
    {
        if (isOpen)
        {
            Debug.Log("Door closed.");
            anim.Play("Close");
        }
    }


}
