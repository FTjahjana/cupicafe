using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Dialogues : MonoBehaviour
{
    public Dialogue[] dialogues;
    private int currentIndex = 0;

    public void TriggerDialogue()
    {
        if (dialogues[currentIndex].sentences.Length != 0 || dialogues[currentIndex].name == "Input")
        {
            DialogueManager.Instance.StartDialogue(dialogues[currentIndex]);
        }

        if (currentIndex + 1 < dialogues.Length) currentIndex++;
    }
    
}
