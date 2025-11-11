using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SystemDialogue : MonoBehaviour
{
    private DialogueChain dialogues;

    void Start()
    {
        StartCoroutine(CallSysDialogue());
    }

    IEnumerator CallSysDialogue()
    {
        yield return new WaitForSeconds(2f);

        dialogues = GetComponent<DialogueChain>();
        if (dialogues != null)
        {
            dialogues.TriggerDialogue();
        }
        yield return null;
    }
    
    

}
