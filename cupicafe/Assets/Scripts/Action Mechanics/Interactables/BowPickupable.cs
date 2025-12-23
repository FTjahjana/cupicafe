using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class BowPickupable : Pickupable
{
    public override void Pickup(Transform hand)
    {PlayerMovement3D pm = GameManager.Instance.Player.GetComponent<PlayerMovement3D>();
        if (!pm.bowModeAllowed){Debug.LogWarning("bowModeAllowed is false"); return;}
        base.Pickup(hand);
        if (!pm.bowModeOn) pm.BowMode(true);
        
    }

    public override void Drop()
    {
        base.Drop();
        PlayerMovement3D pm = GameManager.Instance.Player.GetComponent<PlayerMovement3D>();
        if (pm.bowModeOn) pm.BowMode(false);
    }
}