using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] instructions;
    public Button[] buttons;
    public PlayerMovement3D playerMovementScript;

    [HideInInspector]public bool inGame;
   
    void Start()
    {
        
    }


    void OnEnable() {if (inGame) playerMovementScript.ToggleActions(false);}
    void OnDisable() {if (inGame) playerMovementScript.ToggleActions(true);}

    public void ShowInstruction(int InstructionsIndex)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            instructions[i].SetActive(i == InstructionsIndex);
        }
        EventSystem.current.SetSelectedGameObject(buttons[InstructionsIndex].gameObject);
    }

    public void Play()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            SceneManager.LoadScene("Game");
            GameManager.Instance.inGame = true;
        }
    }

    public void Resume()
    {   
        playerMovementScript.ToggleActions(true);
        // additional button functions in inspector: Main Menu GameObj false & Menu Trigger GameObj true 
        //play pause anim backwards (set up in animator)
        //move the menutrigger thing from the X button to here
    }

    public void Options()
    {
        Debug.Log("Options Placeholder");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        
        // add anims later
    }

}