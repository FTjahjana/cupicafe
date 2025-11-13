using UnityEngine;
using System.Collections.Generic;

public enum QuestStepType
{
    Dialogue,
    AnimatorSequence,
    WaitForCondition
}

[System.Serializable]
public class QuestStep
{
    public string name;
    public QuestStepType type;

    public bool isCompleted;

    public Dialogue dialogue; 
        // used if type == Dialogue

    public Animator animator; public string animatorTrigger;
        // used if type == AnimatorSequence + animator trigger

    public string conditionName; 
        // used if type == WaitForCondition -> What condition are we waiting for?
}
[CreateAssetMenu(menuName = "Game/Quest")]
public class Quest : ScriptableObject
{
    public string questName => name;
    [TextArea] public string description;

    public List<QuestStep> steps;

    public bool isCompleted;

    public float delayAfterCompletion = 3f;


}
