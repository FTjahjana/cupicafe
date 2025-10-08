using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{

    public PlayerInput playerInput;

    public InputAction moveAction;
    public CharacterController characterController;
    public float moveSpeed = 3;

    public InputAction sprintAction;
    public float maxSprintStamina = 5f;
    public float sprintRegenRate = 2f;
    private float sprintStamina;

    public InputAction lookAction;
    public GameObject playerCamera;
    public float lookSensitivity = 2.0f;
    public float upDownRange = 80.0f;
    private float yRotation;

    public InputAction jumpAction;
    public float jumpSpeed = 3.0f;
    public float gravity = 10.0f;
    private Vector3 movingDirection = Vector3.zero;

    public InputAction cursorToggleAction;

    public GameObject cursorObj;


    //public Vector2 moveInput;
    //public Vector3 horizontalMovement;

    // Start is called before the first frame update
    void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Sprint");
        cursorToggleAction = playerInput.actions.FindAction("cursorToggle");

        Cursor.lockState = CursorLockMode.Locked;
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

    void Start()
    {
        sprintStamina = maxSprintStamina;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(moveAction.ReadValue<Vector2>());

        Move();
        Sprint();
        Look();
        cursorToggle();
        Jump();
        
    }

    void Move()
    {
        //take in our WASD or Left Stick values
        Vector2 moveInput = moveAction.ReadValue<Vector2>() * moveSpeed;

        //convert this to a 3D vector 
        Vector3 horizontalMovement = new Vector3(moveInput.x, 0, moveInput.y);

        //accounts for rotation
        horizontalMovement = transform.rotation * horizontalMovement;

        //move the character
        characterController.SimpleMove(horizontalMovement);
    }

    void Sprint()
    {
        bool sprinting = sprintAction.WasPressedThisFrame() && sprintStamina > 0 &&
                        (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);

        moveSpeed = sprinting ? moveSpeed * 2.5f : moveSpeed;
        sprintStamina += (sprinting ? -1 : sprintRegenRate) * Time.deltaTime;
        sprintStamina = Mathf.Clamp(sprintStamina, 0, maxSprintStamina);
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

    void cursorToggle()
    {
        if (cursorToggleAction.WasPressedThisFrame())
        {
            bool isActive = cursorObj.activeSelf;
            cursorObj.SetActive(!isActive);
            
            if (isActive)
            { Cursor.lockState = CursorLockMode.None; lookAction.Disable(); }
            else { Cursor.lockState = CursorLockMode.Locked; lookAction.Enable(); }
        }
    }

    void Jump()
    {

        if (characterController.isGrounded && jumpAction.WasPressedThisFrame())
        {
            movingDirection.y = jumpSpeed;
        }
        movingDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movingDirection * Time.deltaTime);

    }

   
}