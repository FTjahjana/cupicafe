using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    private int currentQuestIndex = 0;
    private Quest CurrentQuest => quests[currentQuestIndex];

    private void Start()
    {
        StartCoroutine(RunQuest(CurrentQuest));
    }

    private IEnumerator RunQuest(Quest quest)
    {
        foreach (QuestStep step in quest.steps)
        {
            switch (step.type)
            {
                case QuestStepType.Dialogue:
                    DialogueManager.Instance.StartDialogue(step.dialogue);
                    yield return new WaitUntil(() => DialogueManager.Instance.dialogueFinished);
                    break;

                case QuestStepType.AnimatorSequence:
                    if (step.animator != null && !string.IsNullOrEmpty(step.animatorTrigger))
                        step.animator.SetTrigger(step.animatorTrigger);
                    // Optional: wait for animation time or signal
                    yield return new WaitForSeconds(2f);
                    break;

                case QuestStepType.WaitForCondition:
                    yield return new WaitUntil(() => QuestConditionMet(step.conditionName));
                    break;
            }
        }

        quest.isCompleted = true;
        yield return new WaitForSeconds(quest.delayAfterCompletion);
        Debug.Log($"Quest '{quest.questName}' complete!");
        Debug.Log("Press C to start next quest...");
    }

    private bool QuestConditionMet(string name)
    {
        // quest-specific condition 
        return  ConditionManager.Instance.IsConditionMet(name);;
    }

    /*private void Update()
    {
        if (CurrentQuest.isCompleted && Input.GetKeyDown(KeyCode.C))
        {
            currentQuestIndex++;
            if (currentQuestIndex < quests.Count)
                StartCoroutine(RunQuest(quests[currentQuestIndex]));
        }
    }*/
}
