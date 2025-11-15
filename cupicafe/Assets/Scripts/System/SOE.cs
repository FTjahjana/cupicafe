using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Specialized;

public class SOE : MonoBehaviour
{ // tag this gameobj lol

    [System.Serializable]
    public class Quest
    { public string questName;  public int numOfQuestParts; }

    [Tooltip("Literally just see the script and edit everything except the Quest names there. It's very straighforward.")]
    int SOEindex; public List<Quest> Quests; public int currentQuestID;
    [Tooltip("Oh yeah fill this one in Inspctor too")]
    [SerializeReference] public List<DialogueType> dialogues;

    void Start()
    {
        currentQuestID = 0;
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
                        StartCoroutine(CallSysDialogue(2, dialogues[0]));
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

            case "Quest1":
                break;

            case "Quest2":
                break;

            default:
                break;
        }
    }

    IEnumerator CallSysDialogue(float preWait, DialogueType dialogue)
    {
        yield return new WaitForSeconds(preWait);

        if (dialogue is Dialogue d)
        {
            DialogueManager.Instance.StartDialogue(d);
        }
        else if (dialogue is Notif_Dialogue n)
        {
            DialogueManager.Instance.StartDialogue(n);
        }
        else if (dialogue is Input_Dialogue i)
        {
            DialogueManager.Instance.StartDialogue(i);
        }
    }
    
}