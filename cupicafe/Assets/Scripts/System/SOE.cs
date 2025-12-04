using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Specialized;

public class SOE : MonoBehaviour
{ // tag this gameobj lol

    [System.Serializable]
    public class Quest
    { public string questName;  public int numOfQuestParts; public List<QuestTrigger> triggers; }

    [Tooltip("Literally just see the script and edit everything except the Quest names there. It's very straighforward.")]
    int SOEindex; public List<Quest> Quests; public int currentQuestID;

    public DialogueManager dialogueManager;
    public SOE_Dialogues SOE_Dialogues; List<DialogueType> dialogues;

    public  List<QuestTrigger> miscTriggers;
    public List<GameObject> relatedGameObjects;

    [HideInInspector] GameObject Player; [SerializeField]PlayerMovement3D playerMove;
    [SerializeField] NPCSpawner Nspawn;

    void Start()
    {
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

    void NextQuest(){ currentQuestID++;
        if (SOEindex + 1 == Quests[currentQuestID].numOfQuestParts)
        {
            Debug.Log("Now moving onto Quest " + Quests[currentQuestID].questName);
            SOEindex = 0; GameManager.Instance.SOEindex = 0;
        } else {Debug.LogError("NextQuest\\(\\) trying to be called when not at last part of the Quest");}
    }

    void RunSOE()
    {
        switch (Quests[currentQuestID].questName)
        {
        case "Tutorial":
            switch (SOEindex)
            {
                case 0:
                    playerMove.ToggleActions(false);
                
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(2, dialogues[0]));
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(3, dialogues[1])); // type: notif

                    miscTriggers[2].gameObject.SetActive(true); //anykey trigger
                    break;

                case 1:
                    dialogueManager.EndDialogue("notif");

                    StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[2])); 
                    miscTriggers[1].gameObject.SetActive(true); //wait for dialogue end trigger
                    break;

                case 2:
                    dialogueManager.nameChangeDue = true;
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[3])); // type: input
                    miscTriggers[1].gameObject.SetActive(true); //wait for input end trigger
                    
                    Debug.unityLogger.logEnabled = false; // this shushes the debug for a sec
                    var t = Quests[0].triggers[10].gameObject; 
                    // this line is here coz it magically fixes the unity borked somehow
                    Debug.unityLogger.logEnabled = true; // turn it back on

                    break;

                case 3:
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[4])); 

                    t = Quests[0].triggers[10].gameObject; 
                    // this line is here coz it magically fixes the unity borked somehow

                    miscTriggers[1].gameObject.SetActive(true); //wait for dialogue end trigger
                    break;

                case 4:
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[5])); // type: notif
                    playerMove.ToggleActions(true); 
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
                    StartCoroutine(SOE_Dialogues.CallSysDialogue(1, dialogues[8])); // type : notif

                    Quests[0].triggers[1].enabled = true; // door open trigger
                    break;

                case 7:
                    dialogueManager.EndDialogue("notif");
                    Nspawn.SpawnPopupNPC(playerMove.behindCounterPos, playerMove.behindCounterRotY);
                    Quests[0].triggers[2].enabled = true; // floor step trigger
                    break;

                case 8:
                    break;
            }
            break;

        case "Quest1":
            break;

        case "Quest2":
            break;

        default:
            break;
        }
    }
    
}