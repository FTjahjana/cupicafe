// SequenceRunner.cs
using UnityEngine;
using System.Collections;

public class SequenceRunner : MonoBehaviour
{
    public QuestSequence sequence;
    private int currentIndex = 0;

    //temp:
    private bool waitingForCompletion = false;
    //end temp

    void Start()
    {
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        foreach (var quest in sequence.quests)
        {
            EventManager.QuestStarted(quest);
            //UNCOMMENT LATER: yield return new WaitUntil(() => currentIndex > 0); // wait for completion signal

            //temp
            waitingForCompletion = true;
            yield return new WaitUntil(() => waitingForCompletion == false);
            //end temp

            yield return new WaitForSeconds(quest.delayAfterCompletion);
            //UNCOMMENT LATER: currentIndex = 0; // reset for next quest
        }

        //temp
        Debug.Log("[Sequence] All quests completed!");
        //end temp
    }
    
        private void Update() // TEMPORARY UPDATE METHOD
    {
        // For testing — press space to complete the current quest
        if (waitingForCompletion && Input.GetKeyDown(KeyCode.Space))
        {
            var quest = sequence.quests[currentIndex];
            CompleteQuest(quest);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        EventManager.QuestCompleted(quest);
        //temp
        waitingForCompletion = false;
        //end temp
        currentIndex++;
    }
}
