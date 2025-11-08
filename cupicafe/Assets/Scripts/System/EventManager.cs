using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class EventManager : MonoBehaviour
{
    public static Action<Quest> OnQuestStarted;
    public static Action<Quest> OnQuestCompleted;

    public static void QuestStarted(Quest quest) => OnQuestStarted?.Invoke(quest);
    public static void QuestCompleted(Quest quest) => OnQuestCompleted?.Invoke(quest);

}
