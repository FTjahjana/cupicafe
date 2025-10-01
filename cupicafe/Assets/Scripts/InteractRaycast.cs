using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractRaycast : MonoBehaviour
{
    public PlayerInput playerInput;
    InputAction interactAction;

    private void Awake()
    {
        interactAction = playerInput.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    { 
        //get forward vector
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        Ray interactRay = new Ray(transform.position, fwd);
        RaycastHit hitData;

        Debug.DrawRay(interactRay.origin, interactRay.direction* 5, Color.green);

        //if our raycast hits something, continue
        if (Physics.Raycast(interactRay, out hitData))
        {
            //checks what kind of thing we hit
            if (hitData.collider.gameObject.layer == 6)
            {
                if (interactAction.triggered)
                {
                    print("CLICKED!");
                }
            }
        }
    }
}
