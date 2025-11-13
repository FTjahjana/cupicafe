using UnityEngine;
using UnityEngine.InputSystem;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private string conditionName;
    [SerializeField] private InputActionReference action;

    private void OnEnable()
    { if (action != null) action.action.performed += OnActionPerformed; }
    private void OnDisable()
    { if (action != null) action.action.performed -= OnActionPerformed; }
    
    public void TriggerCondition()
    {
        //ConditionManager.Instance.SetConditionMet(conditionName);
        GameManager.Instance.IncSOE();
        this.enabled = false;
    }

    // For Dialogue Chain Triggers See: DialogueChain.cs

    // For collider-based Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerCondition();
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        TriggerCondition();
    }
}
