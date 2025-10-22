using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPCInteract : MonoBehaviour, IInteractable
{

    public Animator anim;
    public Text text;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        // start talking to player.
    }

}