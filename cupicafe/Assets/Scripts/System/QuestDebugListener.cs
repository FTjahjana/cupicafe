// QuestDebugListener.cs
using UnityEngine;

public class QuestDebugListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnQuestStarted += OnQuestStarted;
        EventManager.OnQuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        EventManager.OnQuestStarted -= OnQuestStarted;
        EventManager.OnQuestCompleted -= OnQuestCompleted;
    }

    private void OnQuestStarted(Quest quest)
    {
        Debug.Log($"[Event] Quest Started: {quest.questName}");
    }

    private void OnQuestCompleted(Quest quest)
    {
        Debug.Log($"[Event] Quest Completed: {quest.questName}");
    }
}
