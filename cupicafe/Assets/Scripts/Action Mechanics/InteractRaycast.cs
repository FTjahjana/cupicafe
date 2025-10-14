using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractRaycast : MonoBehaviour
{
    public PlayerInput playerInput;
    InputAction interactAction;
    InputAction debugAction;

    public float interactRange = 20;

    bool raydebugLogOn = false;

    public Animator handAnim;

    private void Awake()
    {
        interactAction = playerInput.actions.FindAction("Interact");
        debugAction = playerInput.actions.FindAction("Debug");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get forward vector
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Ray interactRay = new Ray(transform.position, fwd);
        RaycastHit hitData;
        Debug.DrawRay(interactRay.origin, interactRay.direction * interactRange, Color.yellow);
        //if our raycast hits something, continue
        if (Physics.Raycast(interactRay, out hitData))
        {
            //checks what kind of thing we hit
            if (hitData.collider.gameObject.layer == 6)
            {
                if (raydebugLogOn) Debug.Log("Raycast has detected a " + hitData.collider.name);
                if (interactAction.WasPressedThisFrame())
                {
                    Debug.Log("CLICKED!" + hitData.collider.name);
                    Debug.Log("Now Trying to interact with" + hitData.collider.name);
                    if (hitData.collider.TryGetComponent<IInteractable>(out var interactableObject))
                    { interactableObject.Interact(); }
                }
            }
        }

        if (debugAction.WasPressedThisFrame())
        {
            Debug.Log("interactRay.origin: " + interactRay.origin + "interactRay.direction: " + interactRay.direction);
            Debug.Log("transform.position: " + transform.position + "transform.rotation" + transform.rotation);

            raydebugLogOn = !raydebugLogOn;
            Debug.Log("Ray logs toggled!");
        }

    }
}
