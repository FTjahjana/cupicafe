using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private InputActionReference action;
    [SerializeField] private List<InputActionReference> actionList;
    [SerializeField] private bool waitingForDialogueEnd;
    [SerializeField] private bool anyKeyDetect;
    [SerializeField] private bool colliderDetect;

        [SerializeField] private bool disableSelfInsteadOfGameObject;
    
    Collider col;

    private void OnEnable()
    {
        if (waitingForDialogueEnd) {waitForDialogueEnd();
            DialogueManager.Instance.DialogueEnded += ReceivedDialogueEnded;}

        if (colliderDetect) { col.isTrigger = true;}

        if (action != null) action.action.performed += OnActionPerformed;

        if (actionList != null) foreach (var actionReference in actionList)
            { actionReference.action.performed += OnAnyActionPerformed; }
    }

    private void OnDisable()
    {
        if (waitingForDialogueEnd) DialogueManager.Instance.DialogueEnded -= ReceivedDialogueEnded;

        if (colliderDetect) { col.isTrigger = false;}

        if (action != null) action.action.performed -= OnActionPerformed;

        if (actionList != null) foreach (var actionReference in actionList)
            { actionReference.action.performed -= OnAnyActionPerformed; }
    }

    void Start()
    {
        //if (waitingForDialogueEnd) waitForDialogueEnd();
        if (colliderDetect) col = GetComponent<Collider>();
    }

    void Update()
    {
        if (anyKeyDetect) AnyKeyDetect();
    }
    public void TriggerCondition()
    {
        GameManager.Instance.IncSOE();
        if (disableSelfInsteadOfGameObject) this.enabled = false;
        else this.gameObject.SetActive(false);
    }

    // For Dialogue Chain Triggers See: DialogueChain.cs

    // Dialogue Stuff through SOE, etc;
    private void waitForDialogueEnd() { DialogueManager.Instance.triggerSet = true;}
    private void ReceivedDialogueEnded() { if (waitingForDialogueEnd) TriggerCondition(); }

    // For collider-based Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //player collision
            TriggerCondition();
    }

    // For door trigger, check Door.cs

    // For specific action-based Triggers
    private void OnActionPerformed(InputAction.CallbackContext context) { TriggerCondition(); }
    // For a list of action-based Triggers
    private void OnAnyActionPerformed(InputAction.CallbackContext context) { TriggerCondition(); }
    //any key pressed detector
    private void AnyKeyDetect(){ if(Input.anyKeyDown && Input.inputString != "") TriggerCondition(); }
    
}
