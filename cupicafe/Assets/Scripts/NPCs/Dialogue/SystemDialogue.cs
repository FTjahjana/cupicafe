using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SystemDialogue : MonoBehaviour
{
    private NPCDialogueTest dialogueThing;

    void Start()
    {
        StartCoroutine(CallSysDialogue());
    }
    
    IEnumerator CallSysDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueThing = gameObject.GetComponent<NPCDialogueTest>();
        if (dialogueThing != null) { dialogueThing.TriggerDialogue(); }
        yield return null;
    }
}