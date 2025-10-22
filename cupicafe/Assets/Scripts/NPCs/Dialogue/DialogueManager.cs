using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public TextMeshProUGUI nameText, dialogueText;
    private Queue<string> sentences;
    [SerializeField] private GameObject dialoguePanel;

    public Animator animator;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialoguePanel.activeInHierarchy == false) dialoguePanel.SetActive(true);

        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;

        Debug.Log("Started conversation with " + dialogue.name);

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(); return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
    
    void EndDialogue()
    {
        Debug.Log("End of conversation");
        animator.SetBool("isOpen", false);
    }
}
