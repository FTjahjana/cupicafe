using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    public Rigidbody2D rb;
    public float speedMultiplier = 15;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        //moveAction.Enable();
    }

    private void OnDisable()
    {
        //moveAction.Disable();
    }

    void FixedUpdate()
    {
        Move2D();
        //Look();
    }

    void Move2D()
    {
        Vector2 moveInput2D = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveInput2D.x * speedMultiplier, moveInput2D.y * speedMultiplier);
    }
}
