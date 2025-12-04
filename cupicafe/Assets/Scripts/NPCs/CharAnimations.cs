using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimations : MonoBehaviour
{
    public string Name;
    
    [SerializeField] public Animator anim, wingLanim, wingRanim;
    public NPCQueue currentQueue;
    
    public bool isNPC, isInSlot = false, wingOpen;

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

    public void WACToggle()
    {
        WingAnimCall();
        wingOpen = !wingOpen;
    }
    public void WingAnimCall()
    {
        if (!wingOpen)
        {wingLanim.SetTrigger("Open"); wingRanim.SetTrigger("Open");}
        else
        {wingLanim.SetTrigger("Close"); wingRanim.SetTrigger("Close");}
    }
}
