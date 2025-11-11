using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager Instance;
    private HashSet<string> metConditions = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetConditionMet(string name)
    {
        metConditions.Add(name);
        Debug.Log($"Condition '{name}' marked as met.");
    }

    public bool IsConditionMet(string name)
    {
        return metConditions.Contains(name);
    }
    
}
