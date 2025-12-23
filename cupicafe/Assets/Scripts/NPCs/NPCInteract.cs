using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPCInteract : MonoBehaviour, IInteractable
{

    public Animator anim;
    public DialogueChain dialogueChain;
    private bool _talktoPlayer;
    public bool talktoPlayer
    {
        get => _talktoPlayer;
        set
        {
            _talktoPlayer = value;
            if (dialogueChain == null) return;

            if (_talktoPlayer)
            {
                // Subscribe to DialogueManager events
                DialogueManager.Instance.DialogueEnded += dialogueChain.TryAdvanceChain;
                DialogueManager.Instance.DialogueStarted += dialogueChain.OnDialogueStarted;
            }
            else
            {
                // Unsubscribe when player shouldn't talk to this NPC
                DialogueManager.Instance.DialogueEnded -= dialogueChain.TryAdvanceChain;
                DialogueManager.Instance.DialogueStarted -= dialogueChain.OnDialogueStarted;
            }
        }
    }
    public bool shootable = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        dialogueChain = GetComponent<DialogueChain>();
    }

    public void Interact()
    {
        // start talking to player.
        if (dialogueChain!=null && talktoPlayer)dialogueChain.TriggerDialogue();
    }

    void OnDestroy()
    {
        if (dialogueChain != null)
        {
            DialogueManager.Instance.DialogueEnded -= dialogueChain.TryAdvanceChain;
            DialogueManager.Instance.DialogueStarted -= dialogueChain.OnDialogueStarted;
        }
    }

}