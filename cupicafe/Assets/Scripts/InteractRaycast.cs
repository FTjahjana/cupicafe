using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractRaycast : MonoBehaviour
{
    public PlayerInput playerInput;
    InputAction interactAction;

    Transform hand;
    [SerializeField]private Transform handStart, handEnd;
    float handPressDuration = 0.8f;

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
                if (interactAction.WasPressedThisFrame())
                {
                    Debug.Log("Raycast hit"+ hitData.collider.gameObject.name);
                }
            }
        }
        else {
            if (interactAction.WasPressedThisFrame())
                {
                    float t = Mathf.Clamp01((Time.time - startTime) / handPressDuration);

                    hand.transform.position = Vector3.Lerp(handStart.position, handEnd.position, t);
                    hand.transform.rotation = Quaternion.Lerp(handStart.rotation, handEnd.rotation, t);
                    Debug.Log("You just hit air");
                }
        }

    }
}
