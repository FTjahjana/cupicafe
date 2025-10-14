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
    public PlayerMovement3D playerMovementScript; 

    void Start()
    {
        playerMovementScript.ToggleActions(false);
        ShowInstruction(2);
    }

    void OnEnable() {playerMovementScript.ToggleActions(false);}
    void OnDisable() {playerMovementScript.ToggleActions(true);}

    public void ShowInstruction(int InstructionsIndex)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            instructions[i].SetActive(i == InstructionsIndex);
        }
        EventSystem.current.SetSelectedGameObject(buttons[InstructionsIndex].gameObject);
    }

}