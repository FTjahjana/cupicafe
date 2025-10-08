using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject[] instructions;
    public Button[] buttons;
    public PlayerMovement3D playerMovementScript; public InputAction[] actions;

    void Start()
    {
        actions = new InputAction[]
        {
            playerMovementScript.moveAction,
            playerMovementScript.lookAction,
            playerMovementScript.jumpAction
        };

        ToggleActions(false);
        ShowInstruction(2);
    }

    void OnEnable() {ToggleActions(false);}
    void OnDisable() {ToggleActions(true);}

    void ToggleActions(bool thing)
    {
        playerMovementScript.cursorObj.SetActive(thing);
        Cursor.lockState = thing ? CursorLockMode.Locked : CursorLockMode.None;
        foreach (var a in actions)
        if (thing) a.Enable(); else a.Disable();
    }

    public void ShowInstruction(int InstructionsIndex)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            instructions[i].SetActive(i == InstructionsIndex);
        }
        EventSystem.current.SetSelectedGameObject(buttons[InstructionsIndex].gameObject);
    }

}