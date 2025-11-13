using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager Instance;
    public List<string> Conditions;
    int index = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetConditionMet(string name)
    {
        if (index < Conditions.Count && name == Conditions[index])
        {
            index++;
            Conditions.Add(name);
            Debug.Log($"Condition '{name}' marked as met.");
        }
        else Debug.Log($"Can't meet {name} yet.");
    }

    public bool IsConditionMet(string name)
    {
        return Conditions.Contains(name);
    }

    [SerializeField] private string NextCondition => index < Conditions.Count ? Conditions[index] : null;
    
}
