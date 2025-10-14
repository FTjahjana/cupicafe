using UnityEngine;

public class GunPickup : Pickupable
{
    public override void Pickup(Transform hand)
    {
        base.Pickup(hand); // keeps the default pickup behavior
        Debug.Log("Gun equipped! Ready to shoot."); // adds extra behavior
    }
}
