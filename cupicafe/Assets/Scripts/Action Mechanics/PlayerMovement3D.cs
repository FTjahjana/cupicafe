using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement3D : MonoBehaviour
{

    public PlayerInput playerInput;
    private List<InputAction> activityActions = new List<InputAction>();

    public InputAction interactAction;

    [Header("Move")]
    public InputAction moveAction; public InputAction verticalMoveAction;
    public CharacterController characterController;
    public float moveSpeed = 3, baseMoveSpeed = 3;

    [Header("Sprint")]
    public InputAction sprintAction;
    public float maxSprintStamina = 5f;
    public float sprintRegenRate = 2f;
    private float sprintStamina;

    [Header("Look")]
    public InputAction lookAction;
    public GameObject playerCamera;
    [SerializeField] private float lookSensitivityDefault = .5f; public float lookSensitivity;
    public float upDownRange = 80.0f;
    private float yRotation;

    [Header("Jump")]
    public InputAction jumpAction;
    public float jumpSpeed = 3.0f;
    public float gravity = 10.0f;
    private Vector3 movingDirection = Vector3.zero;

    [Header("Fly")]
    public bool isFlying; public bool wingsOut;

    [Header("Bow Mode")]
    public bool bowModeOn; public InputAction bowOffAction, bowOnAction;

    [Header("Cursor Toggle")]
    public InputAction cursorToggleAction;
    public GameObject cursorObj;

    [Header("Stored Positions")]
    public Vector3 behindCounterPos; public float behindCounterRotY;
    public Vector3 startTutPos; public float startTutRotY;


    // Start is called before the first frame update
    void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        verticalMoveAction = playerInput.actions.FindAction("VerticalMove");
        lookAction = playerInput.actions.FindAction("Look");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Shift");
        cursorToggleAction = playerInput.actions.FindAction("cursorToggle");
        bowOffAction = playerInput.actions.FindAction("Previous");
        bowOnAction = playerInput.actions.FindAction("Next");
        interactAction = playerInput.actions.FindAction("Interact");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        sprintStamina = maxSprintStamina;
        lookSensitivity = lookSensitivityDefault;

        activityActions.Add(moveAction); activityActions.Add(verticalMoveAction); activityActions.Add(sprintAction);
        activityActions.Add(lookAction); activityActions.Add(jumpAction); activityActions.Add(cursorToggleAction);
        activityActions.Add(bowOffAction); activityActions.Add(bowOnAction); activityActions.Add(interactAction);

        //temp:
        transform.position = startTutPos; transform.eulerAngles = new Vector3(0,startTutRotY,0);
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
        Fly();
        BowMode();

    }

    public void ToggleActions(bool thing)
    {
        Cursor.lockState = thing ? CursorLockMode.Locked : CursorLockMode.None;
        cursorObj.SetActive(thing);

        foreach (var a in activityActions)
            if (thing) a.Enable(); else a.Disable();
    }

    void Move()
    {
        //take in our WASD or Left Stick values
        Vector2 moveInput = moveAction.ReadValue<Vector2>() * moveSpeed;
        //convert this to a 3D vector 
        Vector3 horizontalMovement = new Vector3(moveInput.x, 0, moveInput.y);
        //accounts for rotation
        horizontalMovement = transform.rotation * horizontalMovement;

        //flight
        if (isFlying)
        {
            float verticalInput = verticalMoveAction.ReadValue<float>();
            float vertical = verticalInput * moveSpeed;
            Vector3 verticalMovement = Vector3.up * vertical;

            Vector3 move = horizontalMovement + verticalMovement;
            characterController.Move(move * Time.deltaTime);
        } else {
            //move the character
            characterController.SimpleMove(horizontalMovement);
        }
    }

    void Sprint()
    {
        if (isFlying || bowModeOn) return;
        bool sprinting = sprintAction.IsPressed() && sprintStamina > 0 &&
                        (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);

        moveSpeed = sprinting ? baseMoveSpeed * 2.5f : baseMoveSpeed;
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
        if (isFlying) return;
        if (characterController.isGrounded && jumpAction.WasPressedThisFrame())
        {
            movingDirection.y = jumpSpeed;
        }
        movingDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movingDirection * Time.deltaTime);

    }

    void Fly()
    {
        /* Putting this here because I'm gonna forget how it works the moment I lay eyes on it for the second time
        and have to go scourging the internet to find this specific article again: 
            https://docs.unity3d.com/Packages/com.unity.inputsystem@1.15/manual/Interactions.html
            relevant sections: #operation, #multitap
        */

        jumpAction.performed += context =>
        {
            if (context.interaction is MultiTapInteraction)
            {   if (isFlying)
                {
                    Debug.Log("flying off");
                    isFlying = false; wingsOut = false;
                }
                else
                {
                    Debug.Log("flying initiated");
                    isFlying = true; wingsOut = true;
                }
            }
        };
    }

    void BowMode()
    {
        interactAction.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                if (!bowModeOn)
                {
                    BowMode(true);
                }
            }
        };

        if (bowOnAction.WasPressedThisFrame() && !bowModeOn)
        {
            BowMode(true);
        }
        else if (bowOffAction.WasPressedThisFrame() && bowModeOn)
        {
            BowMode(false);
        }
    
    }
    void BowMode(bool on)
    {
        Debug.Log("Bowmode " + (on ? "On" : "Off")); bowModeOn = on;
        lookSensitivity = on ? lookSensitivityDefault /20f : lookSensitivityDefault;
        moveSpeed = on ? baseMoveSpeed / 5f : baseMoveSpeed;
    }
}