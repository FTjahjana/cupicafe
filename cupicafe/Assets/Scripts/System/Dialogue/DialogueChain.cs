using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueChain : MonoBehaviour
{   
    [SerializeField]public int currentIndex = 0;
    [SerializeField]public int triggerOnEndCounter = 0;

    public Dialogue[] dialogues; 
    public List<AutoChainSegment> autoChainSegments;

    public List<int> triggerOnEnd; 
    public GameObject DialogueEndTrigger; 
    
    [System.Serializable]
    public struct AutoChainSegment
    {
        public int startIndex; public int endIndex; 
    }

    

    public bool allDialoguesFinished; public bool currentlyChaining;

    public bool CursorUnlockOnStart;
    private PlayerMovement3D pm;
    private float lastTriggerTime;
    public bool DetachPointerWandonStart = true;
    
    [ContextMenu("Reset Dialogue Progress")]
    public void ResetProgress()
    {
        currentIndex = 0;
        currentlyChaining = false; allDialoguesFinished = false;
        triggerOnEndCounter = 0;
        Debug.Log($"{gameObject.name}'s dialogue progress has been reset.");
    }

    void Awake()
    {
        if (DialogueManager.Instance != null)
        {
            //DialogueManager.Instance.DialogueEndedWithSender += TryAdvanceChain;
            //DialogueManager.Instance.DialogueStartedWithSender += OnDialogueStarted;
            DialogueManager.Instance.DialogueEnded += TryAdvanceChain;
            DialogueManager.Instance.DialogueEnded += OnDialogueEnded;
            DialogueManager.Instance.DialogueStarted += OnDialogueStarted;
        }
    }
    void Start()
    {
        pm = GameManager.Instance.Player.GetComponent<PlayerMovement3D>();
    }
    
    void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            //DialogueManager.Instance.DialogueEndedWithSender -= TryAdvanceChain;
            //DialogueManager.Instance.DialogueStartedWithSender -= OnDialogueStarted;
            DialogueManager.Instance.DialogueEnded -= TryAdvanceChain;
            DialogueManager.Instance.DialogueEnded -= OnDialogueEnded;
            DialogueManager.Instance.DialogueStarted -= OnDialogueStarted;
        }
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.Instance.isChainRunning) return;
        if (DialogueManager.Instance.dialoguePanel.activeInHierarchy) return;

        if (Time.time - lastTriggerTime < 0.2f) return; // raycast b too fast smtimes
            lastTriggerTime = Time.time;

        Debug.Log($"TriggerDialogue called for {gameObject.name} | DCindex: {currentIndex} | Time: {Time.time}");

        if (currentlyChaining) return; // prevents another new dialogue from this chain from being called again when its chaining.

        if (currentIndex >= dialogues.Length)
        { Debug.Log($"[{gameObject.name}]: DChain Finished. At DCindex: {currentIndex}"); allDialoguesFinished =true; return; }

        if (CursorUnlockOnStart && pm.cursorLocked) pm.cursorToggle(true);

        AutoChainSegment segment = FindChainSegment(currentIndex);
        bool startingChain = (segment.startIndex == currentIndex);

        if (startingChain)
        {
            currentlyChaining = true;

            DialogueManager.Instance.StartDialogueChain(dialogues, segment.startIndex, segment.endIndex, this, DetachPointerWandonStart);
            currentIndex = segment.endIndex + 1;
        }
        else
        {
            Dialogue dialogue = dialogues[currentIndex];
            DialogueManager.Instance.StartDialogue(dialogue, DetachPointerWandonStart);
            currentIndex++;
        }

        //int activeIndex = currentIndex - 1; 
        string chainingStatus = currentlyChaining ? "Currently chaining" : "Currently not chaining";
        string typeInfo = startingChain  ? $"Chain (1/{segment.endIndex - segment.startIndex + 1})": "Single";

        Debug.Log($"[{gameObject.name}]: {typeInfo} | DCindex: {currentIndex-1} | {chainingStatus}\n" +
              $"{DialogueManager.Instance.CurrentSentenceText}");

        /*if (triggerOnEndCounter < triggerOnEnd.Count &&
        triggerOnEnd[triggerOnEndCounter] == activeIndex)
        {
            DialogueEndTrigger.SetActive(true);
            Debug.Log($"Enabled trigger: {DialogueEndTrigger.name}");
            triggerOnEndCounter++;
        }*/
    }

    public void TryAdvanceChain(string diaType)
    {
        if (diaType != "dialogue") return;

        Debug.Log($"{gameObject.name}: TryAdvanceChain called. DM isChainRunning: {DialogueManager.Instance.isChainRunning}");

        if (!DialogueManager.Instance.isChainRunning && currentlyChaining)
        {
            currentlyChaining = false;

            if (CursorUnlockOnStart && !pm.cursorLocked)pm.cursorToggle(true);
        }
    }

    
    public void OnDialogueStarted(string diaType)
    {
        if (diaType != "dialogue") return;

        Debug.Log($"{gameObject.name}: Dialogue started at DCindex {currentIndex - 1}");

        if (currentlyChaining) return;
        if (triggerOnEndCounter < triggerOnEnd.Count &&
            triggerOnEnd[triggerOnEndCounter] == currentIndex)
        {
            DialogueEndTrigger.SetActive(true);
            Debug.Log($"Enabled trigger: {DialogueEndTrigger.name}");
            triggerOnEndCounter++;
        }
    }

    public void OnDialogueEnded(string diaType)
    {
        if (diaType != "dialogue") return;
        if (currentlyChaining) return;

        Debug.Log($"{gameObject.name}: OnDialogueEnded called.");
        
    }


    private AutoChainSegment FindChainSegment(int index)
    {
        foreach (var segment in autoChainSegments)
        {
            if (segment.startIndex == index)
            { return segment; }
        }
        return new AutoChainSegment { startIndex = -1 }; // indicates “not found.”
    }
}
