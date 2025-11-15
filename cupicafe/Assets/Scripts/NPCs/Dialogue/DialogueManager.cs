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
    public TextMeshProUGUI inputPrompt; public TMP_InputField InputText; public Animator IP_Animator;
    
    [Header("Dialogue Events")]
    public bool dialogueFinished = false; public event Action DialogueEnded;
    public bool triggerSet = false; 

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

            dialogueFinished = false;

            DP_Animator.SetBool("isOpen", true);

            if (dialogue.name == "Player") nameText.text = playerName;
            else nameText.text = dialogue.name;

            Debug.Log("Started conversation with " + dialogue.name);

            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
    }
    public void StartDialogue(Notif_Dialogue notif)
    {
        if (notifPanel.activeInHierarchy == false) notifPanel.SetActive(true);
        //NP_Animator.SetBool("NotifPanelOpen", false);
        //else close, replace text, then open again.
    }
    public void StartDialogue(Input_Dialogue input)
    {
        if (inputPanel.activeInHierarchy == false) inputPanel.SetActive(true);
            //IP_Animator.SetBool("InputPanelOpen", false);
    }
    
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(); return;
        }

        string sentence = sentences.Dequeue();
        if (sentence.Contains("[Player]"))
        {
            sentence = sentence.Replace("[Player]", playerName);
            Debug.Log(sentence);
        }
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        Debug.Log("End of dialogue");
        DP_Animator.SetBool("isOpen", false);
        dialogueFinished = true;

        if (triggerSet) { DialogueEnded?.Invoke(); }
        
        triggerSet = false;
    }

    public void ChangeName() { if (inputPanel.activeInHierarchy) playerName = InputText.text; inputPanel.SetActive(false);}
}
