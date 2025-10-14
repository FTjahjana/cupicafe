using UnityEngine;
using UnityEngine.InputSystem;

public class Pickupable : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    public bool isHeld = false;
    public PlayerInput playerInput; public InputAction dropAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void Start()
    {
        playerInput = GameManager.Instance.Player.GetComponent<PlayerInput>();
        dropAction = playerInput.actions.FindAction("Interact2");
    }

    public virtual void Interact() => Pickup(Hand.Instance.hand.transform);

    public virtual void Pickup(Transform hand)
    {
        transform.GetComponent<Collider>().enabled = false;

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isHeld = true;

        if (rb != null) rb.isKinematic = true;
    }

    public void Drop()
    {
        if (isHeld && dropAction.WasPressedThisFrame())
        {
            transform.SetParent(null);
            if (rb != null) rb.isKinematic = false;

            transform.GetComponent<Collider>().enabled = true;

            isHeld = false;
        }
    }
}
