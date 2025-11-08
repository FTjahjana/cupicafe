using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SystemDialogue : MonoBehaviour
{
    private Dialogues dialogues;

    void Start()
    {
        StartCoroutine(CallSysDialogue());
    }

    IEnumerator CallSysDialogue()
    {
        yield return new WaitForSeconds(2f);

        dialogues = GetComponent<Dialogues>();
        if (dialogues != null)
        {
            dialogues.TriggerDialogue();
        }
        yield return null;
    }
    
    

}
