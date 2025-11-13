using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    private int currentQuestIndex = 0;
    private Quest CurrentQuest => quests[currentQuestIndex];

    private int currentStepIndex = 0;

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
                    step.isCompleted = true;
                    break;

                case QuestStepType.AnimatorSequence:
                    if (step.animator != null && !string.IsNullOrEmpty(step.animatorTrigger))
                        step.animator.SetTrigger(step.animatorTrigger);
                    //uhhh wait
                    yield return new WaitForSeconds(2f);
                    step.isCompleted = true;
                    break;

                case QuestStepType.WaitForCondition:
                    yield return new WaitUntil(() => step.isCompleted);
                    break;
            }
        }

        quest.isCompleted = true;
        yield return new WaitForSeconds(quest.delayAfterCompletion);
        Debug.Log($"Quest '{quest.questName}' complete!");
    }

    public void SetConditionMet(string name)
    {
         if (currentStepIndex >= CurrentQuest.steps.Count) return;

            var step = CurrentQuest.steps[currentStepIndex];
            if (step.name == name)
            {
                step.isCompleted = true;
                Debug.Log($"Step '{name}' completed!");
                currentStepIndex++;
            }
            else
            {
                Debug.LogWarning($"Can't complete '{name}' yet. Next step is '{step.name}'.");
            }
    }

}
