using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueChain : MonoBehaviour
{
    public Dialogue[] dialogues;
    private int currentIndex = 0;
    QuestTrigger questTrigger;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null) { btn.onClick.AddListener(TriggerDialogue); }

        questTrigger = GetComponent<QuestTrigger>();
        DialogueManager.Instance.UnleashTheTrigger += IGotTheTrigger;
    }

    public void TriggerDialogue()
    {
        if (currentIndex < dialogues.Length)
        {
            var dialogue = dialogues[currentIndex];
            if (dialogue.sentences.Length != 0 || dialogue.name == "Input")
                {DialogueManager.Instance.StartDialogue(dialogue);
                Debug.Log("Playing dialogue no." + currentIndex + 1 + "/" + dialogues.Length);
                if(questTrigger != null) { DialogueManager.Instance.triggerSet = true; }
            }
            else
                {Debug.Log("Dialogues missing.. I think");}
        
            currentIndex++;
        }
        
    }
    
    private void IGotTheTrigger()
    {    
        questTrigger.TriggerCondition();
    }

}
