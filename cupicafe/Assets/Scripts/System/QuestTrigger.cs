using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private InputActionReference action;
    [SerializeField] private List<InputActionReference> actionList;
    private PlayerMovement3D pm; 

    [SerializeField] private bool checkFlight;
    [SerializeField] private bool checkFlightOn = true;   
    [SerializeField] private bool checkFlightOff = false; 

    [SerializeField] private bool checkBowMode;
    [SerializeField] private bool checkBowOn = true;      
    [SerializeField] private bool checkBowOff = false; 

    [SerializeField] private bool waitingForDialogueEnd; // P.S. this has NOTHING to do with the DialogueChain version
    [SerializeField] private bool waitingForNotifEnd; [SerializeField] private bool waitingForInputEnd;
    
    [SerializeField] private bool anyKeyDetect;

    [SerializeField] private bool colliderDetect, autoIsTriggerToggle, passColliderDetect;
            private bool playerEntered = false;
    [SerializeField] public Transform targetDest; [SerializeField] private AgentMover agentMover;
    [SerializeField] private bool resetAgent; [SerializeField] private float offsetDistance = .5f;

    [SerializeField] private bool disableSelfInsteadOfGameObject;
    
    [SerializeField] Collider col;

    private void OnEnable()
    {
        pm = GameManager.Instance.Player.GetComponent<PlayerMovement3D>();

        if (waitingForDialogueEnd || waitingForNotifEnd || waitingForInputEnd) {waitForDialogueEnd();
            DialogueManager.Instance.DialogueEnded += ReceivedDialogueEnded;}

        if (colliderDetect && autoIsTriggerToggle) { col.isTrigger = true;}

        if (action != null) action.action.performed += OnActionPerformed;

        if (actionList != null) foreach (var actionReference in actionList)
            { actionReference.action.performed += OnAnyActionPerformed; }
        
        if (checkFlight && pm!= null) pm.OnFlyingToggled += CheckFlight;
        if (checkBowMode && pm != null) pm.OnBowModeToggled += CheckBowMode;

        if (targetDest != null && agentMover != null)
        {   agentMover.enabled = true; gameObject.GetComponent<NavMeshAgent>().enabled = true;
            agentMover.currentDestination = targetDest; agentMover.agent.destination = targetDest.position; }
    }

    private void OnDisable()
    {
        if (waitingForDialogueEnd || waitingForNotifEnd || waitingForInputEnd) DialogueManager.Instance.DialogueEnded -= ReceivedDialogueEnded;

        if (colliderDetect && autoIsTriggerToggle) { col.isTrigger = false;}
        
        if (passColliderDetect) playerEntered = false;

        if (action != null) action.action.performed -= OnActionPerformed;

        if (actionList != null) foreach (var actionReference in actionList)
            { actionReference.action.performed -= OnAnyActionPerformed; }

        if (checkFlight && pm!= null) pm.OnFlyingToggled -= CheckFlight;
        if (checkBowMode && pm != null) pm.OnBowModeToggled -= CheckBowMode;

        if (targetDest != null && agentMover != null)
        { agentMover.currentDestination = null;
        if (resetAgent) {agentMover.enabled = false; gameObject.GetComponent<NavMeshAgent>().enabled = false;}}
    }

    void Awake()
    {
        if (colliderDetect) col = GetComponent<Collider>();
    }

    void Update()
    {
        if (anyKeyDetect) AnyKeyDetect(); 
        if (targetDest != null && agentMover != null) DistanceCheck();
    }
    public void TriggerCondition()
    {   if(!this.enabled) return;
        Debug.Log(gameObject.name +": TriggerCondition() activated");
        GameManager.Instance.IncSOE();
        if (disableSelfInsteadOfGameObject) this.enabled = false;
        else this.gameObject.SetActive(false);
    }

    // For Dialogue Chain Triggers See: DialogueChain.cs, this component is not needed.

    // Dialogue Stuff through SOE, etc;
    private void waitForDialogueEnd() { DialogueManager.Instance.triggerSet = true;}
    private void ReceivedDialogueEnded(string diaType)
    {
        if (diaType == "dialogue" && waitingForDialogueEnd) TriggerCondition();
        else if (diaType == "notif" && waitingForNotifEnd) TriggerCondition();
        else if (diaType == "input" && waitingForInputEnd) TriggerCondition();
    }


    // For collider-based Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; //player collision

        if (colliderDetect) 
        {Debug.Log("colDetect: Player ENTER");
            TriggerCondition();}

        if (passColliderDetect) 
        {Debug.Log("passColDetect: Player ENTER");
            playerEntered = true;}
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (passColliderDetect && playerEntered)
        {
            Debug.Log("passColDetect: Player EXIT");
            TriggerCondition();
        }
    }

    // For door trigger, check Door.cs

    // For specific action-based Triggers
    private void OnActionPerformed(InputAction.CallbackContext context) { TriggerCondition(); }
    // For a list of action-based Triggers
    private void OnAnyActionPerformed(InputAction.CallbackContext context) { TriggerCondition(); }
    //any key pressed detector
    private void AnyKeyDetect(){ if(Input.anyKeyDown && Input.inputString != "") TriggerCondition(); }
    //check flying turned on
    private void CheckFlight(bool isFlying) 
    {
        if ((isFlying && checkFlightOn) || (!isFlying && checkFlightOff)) TriggerCondition();
    
    }
    // check bow mode turned on
    private void CheckBowMode(bool bowOn)
    {
        if ((bowOn && checkBowOn) || (!bowOn && checkBowOff)) TriggerCondition();
    }
    
    
    // for AgentMover . this script must be put on the npc gameobj.
    private void DistanceCheck()
    {
        if (Vector3.Distance(agentMover.transform.position, targetDest.position) < offsetDistance)
        {
            Debug.Log("Quest Trigger: Agent reached target destination");
            TriggerCondition();
        }
    }
    
}
