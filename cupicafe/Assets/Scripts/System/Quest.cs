using UnityEngine;

[CreateAssetMenu(menuName = "Game/Quest")]
public class Quest : ScriptableObject
{
    public string questName => name;
    [TextArea] public string instructions;
    public bool isCompleted;

    public float delayAfterCompletion = 3f;

    public Quest(string title, string instructions)
    {
        this.instructions = instructions;
        isCompleted = false;
    }
}
