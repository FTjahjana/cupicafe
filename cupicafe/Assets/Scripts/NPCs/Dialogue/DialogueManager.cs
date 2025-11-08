using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public TextMeshProUGUI nameText, dialogueText;
    public TMP_InputField InputText;

    private Queue<string> sentences; public string playerName = "Player";
    [SerializeField] private GameObject dialoguePanel, inputPanel;

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
        if (dialogue.name == "Input")
        {
            if (inputPanel.activeInHierarchy == false) inputPanel.SetActive(true);
            //animator.SetBool("InputPanelOpen", false);
        }
        else
        {
            if (dialoguePanel.activeInHierarchy == false) dialoguePanel.SetActive(true);

            animator.SetBool("isOpen", true);

            if (dialogue.name == "Player") nameText.text = playerName;
            else nameText.text = dialogue.name;

            Debug.Log("Started conversation with " + dialogue.name);

            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(); return;
        }

        string sentence = sentences.Dequeue();
        if(sentence.Contains("[Player]"))
        {
            sentence = sentence.Replace("[Player]", playerName);
            Debug.Log(sentence);
        }
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        Debug.Log("End of dialogue");
        animator.SetBool("isOpen", false);
    }

    public void ChangeName() { if (inputPanel.activeInHierarchy) playerName = InputText.text; inputPanel.SetActive(false);}
}
