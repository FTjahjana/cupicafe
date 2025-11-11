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
        if (btn != null) { btn.onClick.AddListener(() => TriggerDialogue()); }

        questTrigger = GetComponent<QuestTrigger>();
    }

    public void TriggerDialogue()
    {
        if (dialogues[currentIndex].sentences.Length != 0 || dialogues[currentIndex].name == "Input")
        {
            DialogueManager.Instance.StartDialogue(dialogues[currentIndex]);
        } else Debug.Log("Dialogues missing.. I think");

        if (currentIndex + 1 < dialogues.Length) currentIndex++;

        if (currentIndex + 1 == dialogues.Length && questTrigger != null) 
            questTrigger.TriggerCondition();
    }
    
}
