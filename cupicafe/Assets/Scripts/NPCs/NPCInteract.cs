using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteract : MonoBehaviour, IInteractable
{
    
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        // start talking to player.
    }

}