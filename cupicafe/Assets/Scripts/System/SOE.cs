using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Specialized;

public class SOE : MonoBehaviour
{ // tag this gameobj lol

    int SOEindex; public int currentQuestID; 
    [System.Serializable]
    public class Quest
    { public string questName;  public int numOfQuestParts; public List<QuestTrigger> triggers; }

    [Header("Assignable Components")]
    [Tooltip("Literally just see the script and edit everything except the assignable components there. It's very straighforward.")]
    public List<Quest> Quests; 

    public DialogueManager dialogueManager; public Hearts hearts; public NPCQueue npcQueue; public PointerWand pointerWand;
    public SOE_Dialogues SOE_Dialogues; List<DialogueType> dialogues;

    public  List<QuestTrigger> miscTriggers;
    /*  0. Movement Trigger
        1. Wait for Dialogue end Trigger
        2. AnyKey Trigger
        3. Jump Trigger
        4. Flight Check Trigger :On
        5. Flight Check Trigger :Off
        6. Bow Mode Check :On
        7. Bow Mode Check :Off
        8. Wait for Notif end Trigger
        9. Wait for Input end Trigger
    */
    public List<GameObject> relatedGameObjects;
    /*  0. Tutorial Colliders
        1. Main Menu Controller
        2. Entrance Door
        3. WorkBox SpawnPoint
        4. Two's targetDest upon spawn (Quest 1).
        5. WorkBox Door
    */

    [HideInInspector] GameObject Player; [SerializeField]PlayerMovement3D pm;
    [SerializeField] NPCSpawner Nspawn;

    private GameObject oneTheNpc, twoTheNpc; 

    void Start()
    {
        //string currentQuest = Quests[currentQuestID].questName;
        Player = GameManager.Instance.Player;

        currentQuestID = 0; dialogues = SOE_Dialogues.dialogues;
        RunSOE();   
        
    }

    public void NewSOE()
    {
        SOEindex = GameManager.Instance.SOEindex;
        Debug.Log("SOEindex: " + SOEindex);

        RunSOE();
    }

    void NextQuest(){ 
        if (SOEindex == Quests[currentQuestID].numOfQuestParts)
        {
            currentQuestID++;
            Debug.Log("Now moving onto Quest "+currentQuestID + ": " + Quests[currentQuestID].questName);
            SOEindex = 0; GameManager.Instance.SOEindex = 0;

            RunSOE();
        } 
        else {Debug.LogError("NextQuest\\(\\) trying to be called when not at last part of the Quest");}
    }

    void RunSOE()
    {
        switch (Quests[currentQuestID].questName){
        case "Tutorial - Baby Steps":
        /*Quest 0 triggers:
            0. Welcmat (collider)
            1. Door Open (door)
            2. Floor Step (collider)
        */
            switch (SOEindex)
            {
                case 0:
                    CurrentQuestNotif();

                    pm.ToggleActions(false);
                    relatedGameObjects[0].SetActive(true); // tut walls
                    relatedGameObjects[1].GetComponent<MainMenu>().toggleOnHold = true; 
                    relatedGameObjects[2].GetComponent<Door>().canClick = false;
                    pm.bowModeAllowed = false; pm.flyingAllowed = false;
                
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(2, dialogues[0]));
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(3, dialogues[1])); // type: notif

                    StartCoroutine(DelayAction(1f, () => 
                    {
                    miscTriggers[2].gameObject.SetActive(true); //anykey trigger
                    }));
                    break;

                case 1:
                    dialogueManager.EndDialogue("notif");

                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[2])); 
                    miscTriggers[1].gameObject.SetActive(true); //wait for dialogue end trigger
                    break;

                case 2:
                    dialogueManager.nameChangeDue = true;
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[3])); // type: input
                    //miscTriggers[9].gameObject.SetActive(true); //wait for input end trigger
                    StartCoroutine(DelayAction(.01f, () => 
                    {
                    miscTriggers[9].gameObject.SetActive(true); //wait for input end trigger
                    }));
                    

                    break;

                case 3:
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[4])); 

                    miscTriggers[1].gameObject.SetActive(true); //wait for dialogue end trigger
                    break;

                case 4:
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[5])); // type: notif
                    relatedGameObjects[1].GetComponent<MainMenu>().toggleOnHold = false;
                    pm.cursorLocked = true;
                    pm.ToggleActions(true);  

                    miscTriggers[0].gameObject.SetActive(true); // movement trigger
                    break;

                case 5:
                    dialogueManager.EndDialogue("notif");

                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[6])); // type: notif 
                    Quests[0].triggers[0].enabled = true; // welcome mat trigger collider
                    break;

                case 6:
                    dialogueManager.EndDialogue("notif");

                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[7]));
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[8])); // type : notif
                    
                    StartCoroutine(DelayAction(.5f, () => 
                    {
                        relatedGameObjects[2].GetComponent<Door>().canClick = true;
                        Quests[0].triggers[1].enabled = true; // door open trigger
                    }));
                    
                    break;

                case 7:
                    dialogueManager.EndDialogue("notif"); dialogueManager.EndDialogue("dialogue");
                    oneTheNpc = Nspawn.SpawnPopupNPC("One", pm.behindCounterPos, pm.behindCounterRotY);
                    togTtp(oneTheNpc,false);
                    pointerWand.AttachToChara(oneTheNpc);

                    oneTheNpc.GetComponent<DialogueChain>().DialogueEndTrigger = miscTriggers[1].gameObject;

                    StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[9])); // type : notif
                    Quests[0].triggers[2].gameObject.SetActive(true); // floor step trigger
                    break;

                case 8:
                    dialogueManager.EndDialogue("notif");
                    togTtp(oneTheNpc,true);

                    StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[10])); //type: notif
                    StartCoroutine(dialogueManager.S2A(true, 1.1f));
                    
                    //wait for dialogue end trigger set by One: ID 2 
                    break;
                case 9:
                    dialogueManager.S2A(false, 0);
                    dialogueManager.EndDialogue("notif");
                    togTtp(oneTheNpc,false);
                    
                    StartCoroutine(DelayAction(.2f, () => 
                    { NextQuest();}));
                    break;

            }
            break;

        case "Who are They?":
        /*Quest 1 triggers:
            0. TwoTheNPC (agentMover targetDest)
            1. Flight Height (collider)
            2. Flight Pass (Double-sided collider)
        */
                switch (SOEindex)
                {
                    case 0:
                        CurrentQuestNotif();

                        twoTheNpc = Nspawn.SpawnPopupNPC("Two", relatedGameObjects[3].transform.position,
                             relatedGameObjects[3].transform.rotation.eulerAngles.y);
                        togTtp(twoTheNpc,false); togTtp(oneTheNpc,false);

                        twoTheNpc.GetComponent<DialogueChain>().DialogueEndTrigger = miscTriggers[1].gameObject;

                        relatedGameObjects[5].GetComponent<Door>().OpenDoor();

                        Quests[1].triggers[0] = twoTheNpc.GetComponent<QuestTrigger>();
                        Quests[1].triggers[0].targetDest = relatedGameObjects[4].transform; 
                        Quests[1].triggers[0].enabled = true; //Two's targetDest agentmover questtrigger.
                        break;

                    case 1:
                        relatedGameObjects[5].GetComponent<Door>().CloseDoor();

                        togTtp(oneTheNpc,true);
                        pointerWand.AttachToChara(oneTheNpc);
                            // dialogue end trigger set by One: ID 3
                        break;

                    case 2:
                        togTtp(oneTheNpc,false); togTtp(twoTheNpc,true);
                        pointerWand.AttachToChara(twoTheNpc);
                        
                            // dialogue end trigger set by Two: ID 2
                        break;

                    case 3:
                            pointerWand.AttachToChara(twoTheNpc);
                            // dialogue end trigger set by Two: ID 3
                        break;

                    case 4:
                        togTtp(twoTheNpc,false);
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[11])); //notif

                        miscTriggers[3].gameObject.SetActive(true); // Jump
                        break;

                    case 5:
                        dialogueManager.EndDialogue("notif");
                        pm.flyingAllowed = true;
                        
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[12])); //notif

                        miscTriggers[4].gameObject.SetActive(true); // Flight Check
                        break;

                    case 6:
                        dialogueManager.EndDialogue("notif");

                        Quests[1].triggers[1].gameObject.SetActive(true); // flight height collider
                        StartCoroutine(Q1_6_FlightCheck());
                        break;

                    case 7:
                        dialogueManager.EndDialogue("notif");
                        togTtp(twoTheNpc,true); pointerWand.AttachToChara(twoTheNpc);
                        // dialogue end trigger set by Two: ID 4
                        break;

                    case 8:
                        togTtp(twoTheNpc,false);

                        // okay imagine a poof to make em closer to you so u can fly past easier
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[16])); //notif
                        Quests[1].triggers[2].gameObject.SetActive(true); // flight pass collider
                        
                        break;

                    case 9:
                        dialogueManager.EndDialogue("notif");
                        
                        togTtp(twoTheNpc,true);
                        pointerWand.AttachToChara(twoTheNpc);
                        // dialogue end trigger set by Two: ID 5
                        break;

                    case 10:
                        NextQuest();
                        break;
                }
            break;

        case "Take a Shot!":
        /*Quest 2 triggers:
            0. TwoTheNPC (agentMover targetDest)
            1. Flight Height (collider)
            2. Flight Pass (Double-sided collider)
        */
                switch (SOEindex)
                {
                    case 0:
                        CurrentQuestNotif();

                        togTtp(oneTheNpc,true); togTtp(twoTheNpc,false);
                        pointerWand.AttachToChara(oneTheNpc);
                        // dialogue end trigger set by One: ID 4
                        break;

                    case 1:
                        togTtp(oneTheNpc,true); pointerWand.AttachToChara(oneTheNpc);
                        pointerWand.PopupText(true, "Ask For Help?");
                        pm.bowModeAllowed = true;
                        pm.bowOnAction.Disable(); pm.bowOffAction.Disable();
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(4, dialogues[18])); //notif
                        
                        Debug.Log("<color=yellow>Trigger task: bow on through left press.</color>");
                        miscTriggers[6].gameObject.SetActive(true); // Bow On
                        break;

                    case 2:
                        togTtp(oneTheNpc,false); pointerWand.PopupText(false);
                        dialogueManager.EndDialogue("notif");

                        StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[19])); //dialogue
                        // do nothing it just runs 
                        StartCoroutine(DelayAction(1.6f, () => 
                        {
                        miscTriggers[8].gameObject.SetActive(true); // wait for dialogue end trigger
                        }));
                        break;

                    case 3:
                        dialogueManager.EndDialogue("dialogue");
                        
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[20])); //notif
                        Debug.Log("<color=yellow>Trigger task: bow on through right press.</color>");
                        miscTriggers[7].gameObject.SetActive(true); // Bow Off
                        break;

                    case 4:
                        NextQuest();
                        break;

                    case 5:
                        break;

                    case 6:
                        break;

                    case 7:
                        break;

                    case 8:
                        break;
                    
                    case 9:
                        StartCoroutine(Q2_9_SysConvo());
                        break;

                    case 10:
                        //NextQuest();
                        break;
                }
            break;

        case "Fire Away!":
            switch (SOEindex)
                {  
                    case 0:
                    CurrentQuestNotif();

                    StartCoroutine(DelayAction(1f, () => 
                    {
                        hearts.gameObject.SetActive(true);
                        npcQueue.StartCoroutine(npcQueue.QueueActive(true));
                        Nspawn.StartCoroutine(Nspawn.SpawnAllRounds());
                        StartCoroutine(DelayAction(5f, () => 
                        {   StartCoroutine(hearts.AttackRoundTimer()); }));
                    }));

                        break;

                    case 1:
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    case 4:
                        break;

                    case 5:
                        break;

                    case 6:
                        break;

                    case 7:
                        break;

                    case 8:
                        break;
                }
            break;

        default:
            break;
        }
    }

    public IEnumerator DelayAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    bool ResetForSkip(int targetQuestID)
    {
        if (currentQuestID >= targetQuestID)
        {
            Debug.LogError("NOT ALLOWED: Trying to skip to previous/current quest.");
            return false;
        }
        Debug.Log($"Skipping to Quest {targetQuestID}: "+ Quests[targetQuestID].questName);
        currentQuestID = targetQuestID;
        SOEindex = 0; GameManager.Instance.SOEindex = 0;

        if (dialogueManager.dialoguePanel.activeInHierarchy)
            dialogueManager.EndDialogue("dialogue");
        if (dialogueManager.notifPanel.activeInHierarchy)
            dialogueManager.EndDialogue("notif");
        if (dialogueManager.inputPanel.activeInHierarchy)
            dialogueManager.EndDialogue("input");

        foreach (var t in miscTriggers)
        {
            t.gameObject.SetActive(false);
            t.enabled = true;
        }
        pointerWand.Detach();

        if (!pm.cursorLocked) pm.cursorToggle(true);
        pm.ToggleActions(true);

        relatedGameObjects[2].GetComponent<Door>().canClick = true;
        
        return true;
    }
    
    [ContextMenu("SKIP / Quest 1")]
    public void SkipToQuest1()
    {
        if (!ResetForSkip(1)) return;

        pm.bowModeAllowed = false; pm.flyingAllowed = false;

        pm.characterController.enabled = false;
        GameManager.Instance.Player.transform.position = npcQueue.CustomerQueue[0].slotLoc.position;
        pm.characterController.enabled = true;

        oneTheNpc = Nspawn.SpawnPopupNPC("One", pm.behindCounterPos, pm.behindCounterRotY);
        var dc = oneTheNpc.GetComponent<DialogueChain>();
        dc.ResetProgress(); dc.currentIndex = 3; togTtp(oneTheNpc,false); 
        dc.triggerOnEndCounter = 1;
        dc.DialogueEndTrigger = miscTriggers[1].gameObject;

        RunSOE();
    }

    [ContextMenu("SKIP / Quest 2")]
    public void SkipToQuest2()
    {
        if (!ResetForSkip(2)) return;

        pm.bowModeAllowed = false; pm.flyingAllowed = true;

        pm.characterController.enabled = false;
        GameManager.Instance.Player.transform.position = npcQueue.CustomerQueue[0].slotLoc.position;
        pm.characterController.enabled = true;

        // One
        oneTheNpc = Nspawn.SpawnPopupNPC("One", pm.behindCounterPos, pm.behindCounterRotY);
        var dc1 = oneTheNpc.GetComponent<DialogueChain>();
        dc1.ResetProgress();  dc1.currentIndex = 4;  togTtp(oneTheNpc,false); 
        dc1.triggerOnEndCounter = 2;
        dc1.DialogueEndTrigger = miscTriggers[1].gameObject;

        // Two
        twoTheNpc = Nspawn.SpawnPopupNPC("Two", relatedGameObjects[4].transform.position, relatedGameObjects[4].transform.rotation.eulerAngles.y);
        var dc2 = twoTheNpc.GetComponent<DialogueChain>();
        dc2.ResetProgress();  dc2.currentIndex = 6;  togTtp(twoTheNpc,false);
        dc2.triggerOnEndCounter = 4;
        dc2.DialogueEndTrigger = miscTriggers[1].gameObject;

        Quests[1].triggers[0] = twoTheNpc.GetComponent<QuestTrigger>();
        Quests[1].triggers[0].targetDest = relatedGameObjects[4].transform; 

        RunSOE();
    }

    [ContextMenu("SKIP / Quest 3")]
    public void SkipToAction() //primarily for testing toward Quest 3: Fire Away! but can be used by player
    {
        if (!ResetForSkip(3)) return;

        pm.bowModeAllowed = true; pm.flyingAllowed = true;

        pm.characterController.enabled = false;
        GameManager.Instance.Player.transform.position = npcQueue.CustomerQueue[0].slotLoc.position;
        pm.characterController.enabled = true;
        
        // One
        oneTheNpc = Nspawn.SpawnPopupNPC("One", pm.behindCounterPos, pm.behindCounterRotY);
        var dc1 = oneTheNpc.GetComponent<DialogueChain>();
        dc1.ResetProgress();  dc1.currentIndex = 4;  togTtp(oneTheNpc,false); 
        dc1.triggerOnEndCounter = 2;
        dc1.DialogueEndTrigger = miscTriggers[1].gameObject;

        // Two
        twoTheNpc = Nspawn.SpawnPopupNPC( "Two", relatedGameObjects[4].transform.position, relatedGameObjects[4].transform.rotation.eulerAngles.y);
        var dc2 = twoTheNpc.GetComponent<DialogueChain>();
        dc2.ResetProgress();  dc2.currentIndex = 6;  togTtp(twoTheNpc,false);
        dc2.triggerOnEndCounter = 4;
        dc2.DialogueEndTrigger = miscTriggers[1].gameObject;

        dialogueManager.NP_Animator.SetBool("S2APopup", false);

        RunSOE();
    }

    void CurrentQuestNotif()
    {
        dialogueManager.qtText.text = "Quest " + currentQuestID + ":";
        var n = new Notif_Dialogue { notifText = Quests[currentQuestID].questName };

        dialogueManager.StartDialogue(n, 2.5f);
        dialogueManager.NP_Animator.SetBool("NewQuest", true);
    }

    void togTtp(GameObject chara, bool thing)
    {
        chara.GetComponent<NPCInteract>().talktoPlayer = thing;
    }
    
    IEnumerator Q1_6_FlightCheck()
    {
        bool played13 = false;
        float onGroundTimer = 0f;

        // ENTRY
        if (pm.isFlying)
        {
            yield return StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[13]));
            played13 = true;
        }
        else
        {
            while (dialogueManager.notifPanel.activeSelf)
                yield return null;

            yield return StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[14]));
        }

        // MAIN LOOP
        while (SOEindex == 6)
        {
            if (!pm.isFlying)
            { onGroundTimer += Time.deltaTime;

                if (onGroundTimer >= 5f)
                { if (!dialogueManager.notifPanel.activeSelf)
                    { yield return StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[17])); }
                    onGroundTimer = 0f;
                }
            }
            else
            { onGroundTimer = 0f;

                if (!played13)
                { if (dialogueManager.notifPanel.activeSelf) dialogueManager.EndDialogue("notif");

                    yield return StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[13]));
                    played13 = true;
                }
                else
                { if (!dialogueManager.notifPanel.activeSelf)
                    { yield return StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[15])); }
                }
            }

            yield return null;
        }
    }

    IEnumerator Q2_9_SysConvo()
    {
        // n
        yield return SOE_Dialogues.CallSysDialogue(0, dialogues[24]);
        yield return new WaitForSeconds(1f);
        dialogueManager.EndDialogue("notif");

        // d
        yield return SOE_Dialogues.CallSysDialogue(0, dialogues[25]);
        yield return new WaitForSeconds(1f);
        dialogueManager.EndDialogue("dialogue");

        // n
        yield return SOE_Dialogues.CallSysDialogue(0, dialogues[25]);
        miscTriggers[8].gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        dialogueManager.EndDialogue("notif");
    }

    #if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 48; style.alignment = TextAnchor.UpperCenter;
        style.normal.textColor = Color.cyan;

        GUI.Label(new Rect(0, 20, Screen.width, 80), $"SOE: ({currentQuestID}, {SOEindex})", style
        );
    }
    #endif

}