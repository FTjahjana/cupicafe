using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    private bool isOpen = false;

    public void Interact()
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "Door opened." : "Door closed.");
    }
}
