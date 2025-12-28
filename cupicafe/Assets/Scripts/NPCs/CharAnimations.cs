using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimations : MonoBehaviour
{
    public string Name; // ehh ill just leave this for now
    
    [SerializeField] public Animator anim;
    [SerializeField] public Animator wingLanim;
    [SerializeField] public Animator wingRanim;

    public NPCQueue currentQueue;
    
    public bool isNPC, isInSlot = false;
    [SerializeField] bool wingOpen;

    [Header("Animations")]
    public string sitAnimation;
    public string standAnimation;
    public string byeAnimation;

    private bool preCutsceneWingLState, preCutsceneWingRState;
    private bool inCutsceneOverride = false;

    void Start()
    {
        if (!isNPC)
        {
            wingLanim.SetBool("Left", true); wingRanim.SetBool("Right", true);
        }
    }

    public void WingAnimCall()
    {   if (!isNPC)
        {
            if (!wingOpen)
            {
                WingTriggererShortcut(2,true);
                wingOpen = true;
            }
            else
            {
                WingTriggererShortcut(2,false);
                wingOpen = false;
            }
        }
    }

    public void CallAnimByTrigger(string trigger)
    {
        if (!anim.isActiveAndEnabled)anim.enabled = true;
        if (trigger == "Bye") {Destroy(gameObject); return;}

        Debug.Log($"Anim trigger fired: {trigger}");
        anim.SetTrigger(trigger);
        
    }

    // Anim Event helper mehtods to call WACO (WingAnimCutsceneOverride)
    public void WACOLO(){Debug.Log("WACOLO");WingAnimCutsceneOverride(1, 0);} // left only open
    public void WACORO(){Debug.Log("WACORO");WingAnimCutsceneOverride(0, 1);} // right only open
    public void WACOBO(){Debug.Log("WACOBO");WingAnimCutsceneOverride(1, 1);} // both open
    public void WACOBC(){Debug.Log("WACOBC");WingAnimCutsceneOverride(2, 2);} // both close

    void WingAnimCutsceneOverride(int Lopen = 0, int Ropen = 0)
    {
        inCutsceneOverride = true;

        preCutsceneWingLState = wingOpen; preCutsceneWingRState = wingOpen;
        if (Lopen == 1) wingLanim.Play("OpenL"); else if (Lopen == 2) wingLanim.Play("CloseL");
        if (Ropen == 1) wingRanim.Play("OpenR"); else if (Ropen == 2) wingRanim.Play("CloseR");
    }

    public void WingAnimCutsceneRestore()
    {
        inCutsceneOverride = false;

        if (preCutsceneWingLState) WingTriggererShortcut(0, true);
        else WingTriggererShortcut(0, false);

        if (preCutsceneWingRState) WingTriggererShortcut(1, true);
        else WingTriggererShortcut(1, false);

        wingOpen = preCutsceneWingLState;
    }


    private void WingTriggererShortcut(int side, bool open)
    { // side: 0 = L, 1 = R, 2 = both
        string state = open ? "Open" : "Close";
             if (side == 0) wingLanim.SetTrigger(state);
        else if (side == 1) wingRanim.SetTrigger(state);
        else if (side == 2) {wingLanim.SetTrigger(state);  wingRanim.SetTrigger(state);}
    }

    public void InstaClose()
    {preCutsceneWingLState = wingOpen; preCutsceneWingRState = wingOpen;
        inCutsceneOverride = true;
        Debug.Log("InstaClose");
        wingLanim.Play("CloseL", 0, 1f); wingRanim.Play("CloseR", 0, 1f);
        wingLanim.Update(0f); wingRanim.Update(0f); 
        }

    public void Highlight(bool on)
    {
        anim.SetBool("IsHighlighted", on);
    }

    public void AnimDisable(){anim.enabled = false;}

}
