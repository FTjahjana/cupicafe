using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{

    public PlayerInput playerInput;
    InputAction moveAction;
    public CharacterController characterController;
    public float speedMultiplier = 3;

    InputAction lookAction;
    public GameObject playerCamera;
    public float lookSensitivity = 2.0f;
    public float upDownRange = 80.0f;
    private float yRotation;

    public float jumpSpeed = 3.0f;
    public float gravity = 10.0f;
    private Vector3 movingDirection = Vector3.zero;

    //public Vector2 moveInput;
    //public Vector3 horizontalMovement;

    // Start is called before the first frame update
    void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        //jumpAction = playerInput.actions.FindAction("Jump");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(moveAction.ReadValue<Vector2>());

       Move();
       Look();
       //Jump();
    }
    
    
    void Move()
    {
        //take in our WASD or Left Stick values
        Vector2 moveInput = moveAction.ReadValue<Vector2>() * speedMultiplier;

        //convert this to a 3D vector 
        Vector3 horizontalMovement = new Vector3(moveInput.x, 0, moveInput.y);

        //accounts for rotation
        horizontalMovement = transform.rotation * horizontalMovement;

        //move the character
        characterController.SimpleMove(horizontalMovement);
    }

    void Look()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float xRotation = lookInput.x * lookSensitivity * Time.deltaTime * 100;
        transform.Rotate(0, xRotation, 0);

        yRotation -= lookInput.y * lookSensitivity * Time.deltaTime * 100;
        yRotation = Mathf.Clamp(yRotation, -upDownRange, upDownRange);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
    } 

    void Jump()
    {
        /*
        if (characterController.isGrounded && jumpAction.WasPressedThisFrame())
        {
                movingDirection.y = jumpSpeed;
        }
            movingDirection.y -= gravity * Time.deltaTime;
            characterController.Move(movingDirection * Time.deltaTime);
        */
    }
}