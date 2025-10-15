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
        Debug.Log(isOpen ? "Door opened." : "Door closed.");
        anim.Play(isOpen ? "Open" : "Close");

    }


}
