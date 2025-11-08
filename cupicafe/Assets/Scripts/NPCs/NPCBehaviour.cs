using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public string Name;
    
    public Animator Animator;
    public NPCQueue currentQueue;
    
    public bool isInSlot = false;

    [Header("Animations")]
    public string sitAnimation;
    public string standAnimation;

    public void LeaveSlot()
    {
        
        if (currentQueue != null)
        {
            currentQueue.ReleaseSlot(transform);
            isInSlot = false;
            currentQueue = null;
        }
    }
}
