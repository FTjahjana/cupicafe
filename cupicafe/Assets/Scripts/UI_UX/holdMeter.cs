using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HoldMeter : MonoBehaviour
{
    public PlayerMovement3D playerMovementScript;
    public PlayerInput playerInput;
    public InputAction attackAction;
    
    [SerializeField] private Image fillImage;
    [SerializeField] private Sprite[] srcSprites;
    private float holdTime = 0f;
    [SerializeField] private float fillSpeed = 2f;
    private bool isHolding = false; 
    public int attackForce = 0;

    void Start()
    {
        fillImage.enabled = false;
        attackAction = playerMovementScript.playerInput.actions.FindAction("Attack");

    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame())
        {
            isHolding = true;
            fillImage.enabled = true;
            holdTime = 0f;
        }

        if (attackAction.IsPressed() && isHolding)
        {
            holdTime += Time.deltaTime * fillSpeed;
            fillImage.fillAmount = Mathf.Min(holdTime, 1f);

            int index = Mathf.FloorToInt(fillImage.fillAmount * srcSprites.Length) - 1;
            if (index >= 0 && index < srcSprites.Length)
            {
                fillImage.sprite = srcSprites[index];
                attackForce = index;
            }
        }

        if (attackAction.WasReleasedThisFrame())
        {
            isHolding = false;
            fillImage.fillAmount = 0f;
            fillImage.sprite = srcSprites[0];
        }

    }
}
