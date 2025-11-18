using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private InputActionReference action;
    [SerializeField] private List<InputActionReference> actionList;
    [SerializeField] private bool waitingForDialogueEnd;

    private void OnEnable()
    {
        DialogueManager.Instance.DialogueEnded += ReceivedDialogueEnded;

        if (action != null) action.action.performed += OnActionPerformed;

        if (actionList != null) foreach (var actionReference in actionList)
            { actionReference.action.performed += OnAnyActionPerformed; }
    }

    private void OnDisable()
    {
        if (action != null) action.action.performed -= OnActionPerformed;

        if (actionList != null) foreach (var actionReference in actionList)
            { actionReference.action.performed -= OnAnyActionPerformed; }
    }

    void Start()
    {
        if (waitingForDialogueEnd) waitForDialogueEnd();
    }

    public void TriggerCondition()
    {
        GameManager.Instance.IncSOE();
        this.enabled = false;
    }

    // For Dialogue Chain Triggers See: DialogueChain.cs

    // Dialogue Stuff through SOE, etc;
    private void waitForDialogueEnd() { DialogueManager.Instance.triggerSet = true;}
    private void ReceivedDialogueEnded() { if (waitingForDialogueEnd) TriggerCondition(); }

    // For collider-based Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerCondition();
    }

    // For specific action-based Triggers
    private void OnActionPerformed(InputAction.CallbackContext context) { TriggerCondition(); }
    // For a list of action-based Triggers
    private void OnAnyActionPerformed(InputAction.CallbackContext context) { TriggerCondition(); }
}
