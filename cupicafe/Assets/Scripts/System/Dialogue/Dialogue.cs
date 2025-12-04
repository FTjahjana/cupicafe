using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueType { }

[System.Serializable]
public class Dialogue : DialogueType
{
    public string name;
    [TextArea(3, 8)] public string[] sentences;
}

[System.Serializable]
public class Notif_Dialogue : DialogueType
{
    [TextArea(1, 1)] public string notifText;
}

[System.Serializable]
public class Input_Dialogue : DialogueType
{
    public string inputTitle;
    [TextArea(3, 5)] public string inputPrompt;
}