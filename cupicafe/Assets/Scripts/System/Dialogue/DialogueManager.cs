using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public bool ultraDebuggerOn;

    private Queue<string> sentences; public string playerName = "Player";

    [Header("Dialogue Panel")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText, dialogueText; public Animator DP_Animator;

    public string CurrentSentenceText => dialogueText != null ? dialogueText.text : "No Text";

    [Header("Notif Panel")]
    public GameObject notifPanel;
    public TextMeshProUGUI notifText; public TextMeshProUGUI qtText;
    public Animator NP_Animator;

    [Header("Input Panel")]
    public GameObject inputPanel;
    public TextMeshProUGUI inputTitle; public TextMeshProUGUI inputPrompt; 
    public TMP_InputField InputText; public Animator IP_Animator; public bool typeInputCont = false;
    
    [Header("Dialogue Events")]
    public bool dialogueFinished = false; 
    public event Action<string> DialogueEnded; 
    public event Action<string> DialogueStarted; 
    public event Action<DialogueChain, string> DialogueEndedWithSender; 
    public event Action<DialogueChain, string> DialogueStartedWithSender;
    public bool triggerSet = false; public bool nameChangeDue; 
    
    private Queue<Dialogue> dialogueChainQueue; 
    public bool isChainRunning = false;
    public bool isDialogueFromChain = false;
    private int totalChainCount;
    private int currentChainProgress;
    private DialogueChain activeDialogueChain;

    public PointerWand pointerWand;

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
        dialogueChainQueue = new Queue<Dialogue>();
    }

    public void StartDialogue(Dialogue dialogue, bool DetachPointerWandonStart = false)
    {if (ultraDebuggerOn)Debug.Log("StartDialogue(dialogue) in motion");
    DialogueStarted?.Invoke("dialogue");

        if (dialoguePanel.activeInHierarchy == false) dialoguePanel.SetActive(true);
        DP_Animator.SetTrigger("Open");

        dialogueFinished = false;
        
        if (dialogue.name == "Player") nameText.text = playerName;
        else nameText.text = dialogue.name;

        Debug.Log("Convo with: " + dialogue.name);
        isDialogueFromChain = false;

        if (DetachPointerWandonStart && pointerWand.attached) pointerWand.Detach();

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void StartDialogue(Notif_Dialogue notif)
    {if (ultraDebuggerOn)Debug.Log("StartDialogue(notif) in motion");
    DialogueStarted?.Invoke("notif");

        if (notifPanel.activeInHierarchy == false) {notifPanel.SetActive(true);
        notifText.text = notif.notifText;
        
        NP_Animator.SetTrigger("Open"); 
        
        dialogueFinished = false;
        isDialogueFromChain = false;}
        else {Debug.LogError("Notif Panel is open when it's not supposed to be."); NP_ForceReset();
        StartDialogue(notif);}
    }
    public void StartDialogue(Notif_Dialogue notif, float timer)
    {if (ultraDebuggerOn)Debug.Log("StartDialogue(notif) in motion");
    DialogueStarted?.Invoke("notif");

        if (notifPanel.activeInHierarchy == false) {notifPanel.SetActive(true);
        notifText.text = notif.notifText;

        NP_Animator.SetTrigger("Open");
        
        dialogueFinished = false;
        isDialogueFromChain = false;}
        else {Debug.LogError("Notif Panel is open when it's not supposed to be."); NP_ForceReset();
        StartDialogue(notif, timer);}

        StartCoroutine(AutoCloseNotif(timer));
    }
    public void StartDialogue(Input_Dialogue input)
    {if (ultraDebuggerOn)Debug.Log("StartDialogue(input) in motion");
    DialogueStarted?.Invoke("input");

        if (inputPanel.activeInHierarchy == false) {inputPanel.SetActive(true);
        inputTitle.text = input.inputTitle;
        inputPrompt.text = input.inputPrompt;
        IP_Animator.SetTrigger("Open");
        
        dialogueFinished = false;
        isDialogueFromChain = false;}
        else {Debug.LogError("Input Panel is open when it's not supposed to be.");}
    }
    
    public void StartDialogueChain(Dialogue[] chain, int chainStart, int chainEnd, DialogueChain ownerChain, bool DetachPointerWandonStart = false) //end inclusive
    {  if (ultraDebuggerOn)Debug.Log("StartDialogueChain() in motion");
        DialogueStarted?.Invoke("dialogue");

        totalChainCount = (chainEnd - chainStart) + 1;
        currentChainProgress = 0;

        activeDialogueChain = ownerChain;

        if (DetachPointerWandonStart && pointerWand.attached) pointerWand.Detach();
        
        sentences.Clear();
        dialogueChainQueue.Clear();
        isChainRunning = true;

        for (int i=chainStart; i<=chainEnd; i++)
        {dialogueChainQueue.Enqueue(chain[i]);}
            StartNextChainDialogue();
        
    }

    public void DisplayNextSentence()
    {if (ultraDebuggerOn)Debug.Log("DisplayNextSentence() in motion");
        if (sentences.Count == 0)
        {
            if (isDialogueFromChain && isChainRunning && dialogueChainQueue.Count > 0){ StartNextChainDialogue(); return; } 
            else if (typeInputCont) {EndDialogue("input"); return;}
            else {EndDialogue("dialogue"); return;}
        }

        string sentence = sentences.Dequeue();
        if (sentence.Contains("[Player]"))
        {
            sentence = sentence.Replace("[Player]", playerName);
        }
        dialogueText.text = sentence;
    }

    private void StartNextChainDialogue()
{   if (ultraDebuggerOn)Debug.Log("StartNextChainDialogue() in motion");
    if (dialogueChainQueue.Count > 0)
    {
        currentChainProgress++;

        if (currentChainProgress == totalChainCount &&
            activeDialogueChain != null && activeDialogueChain.DialogueEndTrigger != null)
        {
            Debug.Log("[DEBUG] Last dialogue in chain STARTED → enabling Dialogue End Trigger");
            activeDialogueChain.DialogueEndTrigger.SetActive(true);
        }

        Dialogue nextDialogue = dialogueChainQueue.Dequeue();
        isDialogueFromChain = true;

        if (isChainRunning)Debug.Log($"Chain Progress: ({currentChainProgress}/{totalChainCount})");
        
        if (dialoguePanel.activeInHierarchy == false) dialoguePanel.SetActive(true);
        DP_Animator.SetTrigger("Open"); dialogueFinished = false;

        if (nextDialogue.name == "Player") nameText.text = playerName; 
        else nameText.text = nextDialogue.name;
        if (ultraDebuggerOn) Debug.Log("Convo with: " + nextDialogue.name);

        sentences.Clear();
        foreach (string sentence in nextDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    else
    {
        EndDialogue("dialogue");
    }
}

    public void EndDialogue(string diaType)
    {if (ultraDebuggerOn)Debug.Log("EndDialogue() in motion");
        switch (diaType)
        {
            case "dialogue":
                if (ultraDebuggerOn)Debug.Log("End of dialogue");
                DP_Animator.SetTrigger("Close"); dialoguePanel.SetActive(false);

                isChainRunning = false;
                break;

            case "notif":
                if (ultraDebuggerOn)Debug.Log("End of notif");
                NP_Animator.SetBool("NewQuest", false); 
                NP_Animator.SetTrigger("Close"); notifPanel.SetActive(false);
                break;

            case "input":
                if (ultraDebuggerOn)Debug.Log("End of input");
                if (nameChangeDue) {playerName = InputText.text; nameChangeDue = false; 
                    Debug.Log("Player name is now "+ playerName);}
                    
                IP_Animator.SetTrigger("Close"); inputPanel.SetActive(false);
                typeInputCont = false;
                break;
        }
        dialogueFinished = true;
        DialogueEnded?.Invoke(diaType);
        triggerSet = false;
    }

    public void SetTIC(bool tic) => typeInputCont = tic;

    private IEnumerator AutoCloseNotif(float t)
    {yield return new WaitForSeconds(t); EndDialogue("notif");}

    private void NP_ForceReset()
    {   Debug.Log("NP_ForceReset initiated.");
        StopCoroutine(AutoCloseNotif(0));

        if (!notifPanel.activeSelf) notifPanel.SetActive(true); 
        if (!NP_Animator.enabled) NP_Animator.enabled = true;

        NP_Animator.ResetTrigger("Open"); NP_Animator.ResetTrigger("Close");
        NP_Animator.SetBool("NewQuest", false); NP_Animator.SetBool("S2APopup", false);

        NP_Animator.SetTrigger("Close"); notifPanel.SetActive(false);
    }

    public IEnumerator S2A(bool state, float delay) 
    { yield return new WaitForSeconds(delay); NP_Animator.SetBool("S2APopup", state);}
}
