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

    public SOE_Dialogues SOE_Dialogues; List<DialogueType> dialogues;

    void Start()
    {
        currentQuestID = 0; dialogues = SOE_Dialogues.dialogues;
        RunSOE();   
        
    }

    public void NewSOE()
    {
        SOEindex = GameManager.Instance.SOEindex;
        Debug.Log("SOEindex is now" + SOEindex);

        if (SOEindex + 1 == Quests[currentQuestID].numOfQuestParts)
        {
            currentQuestID++;
            Debug.Log("Now moving onto Quest " + Quests[currentQuestID].questName);
            SOEindex = 0;  GameManager.Instance.SOEindex = 0;
        }

        RunSOE();
    }

    void RunSOE()
    {
        switch (Quests[currentQuestID].questName)
        {
            case "Tutorial":
                switch (SOEindex)
                {
                    case 0:
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(2, dialogues[0]));
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[1])); // notif

                        Quests[0].triggers[0].enabled = true; //anykey trigger
                        break;

                    case 1:
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(2, dialogues[2]));
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[3])); // input
                        break;

                    case 2:
                        StartCoroutine(SOE_Dialogues.CallSysDialogue(0, dialogues[4]));
                        Quests[0].triggers[1].enabled = true; // movement trigger
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

            case "Quest1":
                break;

            case "Quest2":
                break;

            default:
                break;
        }
    }
    
}