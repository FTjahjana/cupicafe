using UnityEngine;
using UnityEngine.InputSystem;

public class Book : MonoBehaviour, IInteractable
{
    
    private bool isOpen = false;
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!isOpen) OpenBook();
    }

    public void OpenBook()
    {
        Debug.Log("Book opening.");
        anim.SetTrigger("Open");
    }

    public void CloseBook()
    {
        Debug.Log("Book closing.");
        anim.SetTrigger("Close");
    }

}