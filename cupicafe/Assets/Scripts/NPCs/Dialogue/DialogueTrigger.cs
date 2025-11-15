using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeReference] // allows polymorphic serialization
    public List<DialogueType> dialogues;

    public float delay = 1f;

    // Call this to trigger a specific dialogue in the list
    public void TriggerDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Count) return;
        StartCoroutine(CallSysDialogue(delay, dialogues[index]));
    }

    private IEnumerator CallSysDialogue(float preWait, DialogueType dialogue)
    {
        yield return new WaitForSeconds(preWait);

        if (dialogue is Dialogue d)
            DialogueManager.Instance.StartDialogue(d);
        else if (dialogue is Notif_Dialogue n)
            DialogueManager.Instance.StartDialogue(n);
        else if (dialogue is Input_Dialogue i)
            DialogueManager.Instance.StartDialogue(i);
    }
}
