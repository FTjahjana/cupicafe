using UnityEngine;
using UnityEngine.InputSystem;

public class Book : MonoBehaviour, IInteractable
{
    
    public void Interact()
    {
        Debug.Log("You have touched the book.");
    }
}