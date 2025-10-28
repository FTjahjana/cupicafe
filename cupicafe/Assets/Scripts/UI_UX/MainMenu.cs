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

    [SerializeField] private SceneLoader sceneLoader;
    
    void Start()
    {
        playerMovementScript.ToggleActions(false);
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

    public void Play()
    {
        // if current scene = "Game"
        sceneLoader.LoadScene("Game");
    }

    public void Resume()
    {
        //play pause anim backwards (set up in animator)
        this.gameObject.SetActive(false);
        //move the menutrigger thing from the X button to here
    }

    public void Options()
    {
        Debug.Log("Options Placeholder");
    }

    public void ExitGame()
    {
        sceneLoader.QuitGame(); 
        // add anims later
    }

}