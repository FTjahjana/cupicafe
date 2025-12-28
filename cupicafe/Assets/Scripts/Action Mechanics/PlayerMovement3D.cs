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
    public InputAction lookAction; public bool canLook = true;
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
    public bool flyingAllowed = true, isFlying; public CharAnimations charAnimations;
    public event Action<bool> OnFlyingToggled; public bool G_Effective = true;

    [Header("Bow Mode")]
    public bool bowModeAllowed = true, bowModeOn; public InputAction bowOffAction, bowOnAction;
    public Pickupable bowPickupableComp; public event Action<bool> OnBowModeToggled;

    [Header("Cursor Toggle")]
    public InputAction cursorToggleAction;
    public GameObject cursorObj; public bool cursorLocked = true;

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
            jumpAction.performed += OnJumpFlyPerformed;
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
        if (canLook) Look();
        cursorToggle();
        //Jump();
        //Fly();
        BowMode();

        ApplyGravity();

    }

    public void ToggleActions(bool thing)
    {   
        foreach (var a in activityActions)
            {if (thing) a.Enable(); else a.Disable();}
            
        canLook = thing;

        if (thing)
        {
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            cursorObj.SetActive(cursorLocked); 
            if (!cursorLocked) lookAction.Disable(); else lookAction.Enable();
        }
        else
        {
            lookAction.Disable();
            Cursor.lockState = CursorLockMode.None;
            cursorObj.SetActive(false); 
        }    
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
            movingDirection.y += verticalInput * moveSpeed * Time.deltaTime;
            movingDirection.y = Mathf.Clamp(movingDirection.y, -moveSpeed, moveSpeed);

            Vector3 move = horizontalMovement + Vector3.up * movingDirection.y;
            characterController.Move(move * Time.deltaTime);
        } else {
            //move the character
            characterController.Move(horizontalMovement * Time.deltaTime);
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

    public void cursorToggle(bool fromOuterScript = false)
    {
        if (cursorToggleAction.WasPressedThisFrame() || fromOuterScript)
        {
            bool isActive = cursorObj.activeSelf;
            cursorObj.SetActive(!isActive);

            if (isActive)
            { Cursor.lockState = CursorLockMode.None; lookAction.Disable(); cursorLocked = false;}
            else { Cursor.lockState = CursorLockMode.Locked; lookAction.Enable(); cursorLocked = true;}
        }
    }

    /*void Jump()
    {
        if (isFlying) return;
        if (characterController.isGrounded && jumpAction.WasPressedThisFrame())
        {
            movingDirection.y = jumpSpeed;
        }
        movingDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movingDirection * Time.deltaTime);

    }*/
    private void OnJumpFlyPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"[JUMPFLY DEBUG] Interaction = {context.interaction?.GetType().Name}");

        if (context.interaction is TapInteraction)
        {
            if (characterController.isGrounded)
            {
                if (isFlying) return;
                movingDirection.y = jumpSpeed;
            }
        }
        else if (context.interaction is MultiTapInteraction)
        {   if (!flyingAllowed) return;
        charAnimations.WingAnimCall();
            
            if (isFlying)
            {
                Debug.Log("flying off");
                isFlying = false; 
            }
            else
            {
                Debug.Log("flying initiated");
                isFlying = true; 
                movingDirection.y += jumpSpeed*2;
            }

            OnFlyingToggled?.Invoke(isFlying);
        }
    }

    /*void Fly()
    {
        /* Putting this here because I'm gonna forget how it works the moment I lay eyes on it for the second time
        and have to go scourging the internet to find this specific article again: 
            https://docs.unity3d.com/Packages/com.unity.inputsystem@1.15/manual/Interactions.html
            relevant sections: #operation, #multitap
        /

        jumpAction.performed += context =>
        {if (!flyingAllowed) return;
            if (context.interaction is MultiTapInteraction)
            {   charAnimations.WingAnimCall();
                
                if (isFlying)
                {
                    Debug.Log("flying off");
                    isFlying = false; 
                }
                else
                {
                    Debug.Log("flying initiated");
                    isFlying = true; 
                }

                OnFlyingToggled?.Invoke(isFlying);
            }
        };
    }*/

    void ApplyGravity()
    {
        if (!isFlying)
        {
            if (!characterController.isGrounded)
                movingDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            if (G_Effective) movingDirection.y -= (gravity * 0.05f) * Time.deltaTime;
        }

        characterController.Move(movingDirection * Time.deltaTime);
    }

    void BowMode()
    { if (!bowModeAllowed) return;
        interactAction.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            { if (!bowModeOn)BowMode(true, true); }
        };

        if (bowOnAction.WasPressedThisFrame() && !bowModeOn)
        {
            BowMode(true, true);
        }
        else if (bowOffAction.WasPressedThisFrame() && bowModeOn)
        {
            BowMode(false, true);
        }
    
    }
    public void BowMode(bool on,  bool overrideHand = false)
    {
        if (bowModeOn == on) return; //why u try turn it on when it alreaydy on

        bowModeOn = on; Debug.Log("Bowmode " + (on ? "On" : "Off")); 
        lookSensitivity = on ? lookSensitivityDefault /20f : lookSensitivityDefault;
        moveSpeed = on ? baseMoveSpeed / 2.5f : baseMoveSpeed;

        if (!overrideHand) return; 

        Transform hand = GameManager.Instance.hand;
        if (on)
        {
            if (hand.childCount > 1) hand.GetChild(1).GetComponent<Pickupable>().Drop();
            bowPickupableComp.Interact();
        }

        if (!on)
        {
            hand.Find("bow").GetComponent<Pickupable>().Drop();
        }

        OnBowModeToggled?.Invoke(on);
    }
}