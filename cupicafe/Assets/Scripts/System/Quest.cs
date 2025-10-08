[System.Serializable]
public class Quest
{
    public string title;
    public string instructions;
    public bool isCompleted;

    public Quest(string title, string instructions)
    {
        this.title = title;
        this.instructions = instructions;
        isCompleted = false;
    }
}
