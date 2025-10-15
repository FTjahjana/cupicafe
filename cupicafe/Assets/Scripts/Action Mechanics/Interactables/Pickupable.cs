using UnityEngine;
using UnityEngine.InputSystem;

public class Pickupable : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    public bool isHeld = false;
    public PlayerInput playerInput; public InputAction dropAction;

    public bool useCustomTilt; public Quaternion customTilt;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        playerInput = GameManager.Instance.Player.GetComponent<PlayerInput>();
        dropAction = playerInput.actions.FindAction("Interact2");
    }

    void Update(){
        if (dropAction.WasPressedThisFrame()) Drop();
    }

    public virtual void Interact()
    {
        if (GameManager.Instance.handIsHolding == false) { Pickup(GameManager.Instance.hand.transform); }
        else { Debug.Log("Hand already holding something"); } }

    public virtual void Pickup(Transform hand)
    {
        transform.GetComponent<Collider>().enabled = false;

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        if (useCustomTilt) transform.localRotation = customTilt;
        else transform.localRotation = Quaternion.identity;

        isHeld = true; GameManager.Instance.handIsHolding = true;

        if (rb != null) rb.isKinematic = true;
    }

    public void Drop()
    {
        if (isHeld)
        {
            Debug.Log("Dropped a" + gameObject.name);
            transform.SetParent(null);
            if (rb != null) rb.isKinematic = false;

            transform.GetComponent<Collider>().enabled = true;

            isHeld = false; GameManager.Instance.handIsHolding = false;
        }
    }
}
