using UnityEngine;

public class Fruit : Pickupable
{
    public FruitData data;

    private void Start()
    {
        GetComponent<Renderer>().material.color = data.color;
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log($"Picked up a {data.name}!");
    }
}
