using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private Queue<string> sentences; public string playerName = "Player";

    [Header("Dialogue Panel")]
    [SerializeField] private GameObject dialoguePanel;
    public TextMeshProUGUI nameText, dialogueText; public Animator DP_Animator;

    [Header("Notif Panel")]
    [SerializeField] private GameObject notifPanel;
    public TextMeshProUGUI notifText; public Animator NP_Animator;

    [Header("Input Panel")]
    [SerializeField] private GameObject inputPanel;
    public TextMeshProUGUI inputTitle; public TextMeshProUGUI inputPrompt; 
    public TMP_InputField InputText; public Animator IP_Animator;
    
    [Header("Dialogue Events")]
    public bool dialogueFinished = false; public event Action DialogueEnded;
    public bool triggerSet = false; public bool nameChangeDue;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
            if (dialoguePanel.activeInHierarchy == false) dialoguePanel.SetActive(true);
            DP_Animator.SetTrigger("Open");

            dialogueFinished = false;
            
            if (dialogue.name == "Player") nameText.text = playerName;
            else nameText.text = dialogue.name;

            Debug.Log("Convo with: " + dialogue.name);

            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
    }
    public void StartDialogue(Notif_Dialogue notif)
    {
        if (notifPanel.activeInHierarchy == false) {notifPanel.SetActive(true);
        notifText.text = notif.notifText;
        NP_Animator.SetTrigger("Open");
        
        dialogueFinished = false;}
        else {Debug.LogError("Notif Panel is open when it's not supposed to be.");}
    }
    public void StartDialogue(Input_Dialogue input)
    {
        if (inputPanel.activeInHierarchy == false) {inputPanel.SetActive(true);
        inputTitle.text = input.inputTitle;
        inputPrompt.text = input.inputPrompt;
        IP_Animator.SetTrigger("Open");
        
        dialogueFinished = false;}
        else {Debug.LogError("Input Panel is open when it's not supposed to be.");}
    }
    
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue("dialogue"); return;
        }

        string sentence = sentences.Dequeue();
        if (sentence.Contains("[Player]"))
        {
            sentence = sentence.Replace("[Player]", playerName);
            Debug.Log(sentence);
        }
        dialogueText.text = sentence;
    }

    public void EndDialogue(string diaType)
    {
        switch (diaType)
        {
            case "dialogue":
                Debug.Log("End of dialogue");
                DP_Animator.SetTrigger("Close"); dialoguePanel.SetActive(false);
                break;

            case "notif":
                Debug.Log("End of notif");
                NP_Animator.SetTrigger("Close"); notifPanel.SetActive(false);
                break;

            case "input":
                Debug.Log("End of input");
                if (nameChangeDue) {playerName = InputText.text; nameChangeDue = false; 
                    Debug.Log("Player name is now "+ playerName);}
                    
                IP_Animator.SetTrigger("Close"); inputPanel.SetActive(false);
                break;
        }
        dialogueFinished = true;
        if (triggerSet) { DialogueEnded?.Invoke(); }
        triggerSet = false;
    }

}
