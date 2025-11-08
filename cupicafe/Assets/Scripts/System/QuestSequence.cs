using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Quest Sequence")]
public class QuestSequence : ScriptableObject
{
    public List<Quest> quests = new List<Quest>();
}
