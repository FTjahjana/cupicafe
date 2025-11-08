using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<Quest> Quests = new List<Quest>();

    public GameObject Target1, Target2;

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

    public void AddQuest(Quest quest)
    {
        if (!Quests.Contains(quest))
        {
            Quests.Add(quest);
            Debug.Log($"Quest added: {quest.questName}");
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (Quests.Contains(quest))
        {
            Quests.Remove(quest);
            Debug.Log($"Quest completed: {quest.questName}");
        }
    }

    public bool HasQuest(Quest quest) => Quests.Contains(quest);
}
