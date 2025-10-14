using UnityEngine;


public class ArrowController : MonoBehaviour
{
    [SerializeField] private float dragCoefficient = 1.8f;
    [SerializeField] private float length = 0.7f;

    private Rigidbody rB;
    private float dragOffset;

    public Rigidbody RB => rB;
    public float Length => length;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
        dragOffset = length * 0.4f;
    }

    void FixedUpdate()
    {
        float vel = rB.linearVelocity.magnitude;
        float drag = dragCoefficient * vel * vel * 0.001f;
        rB.AddForceAtPosition(-rB.linearVelocity.normalized * drag,
            rB.transform.TransformPoint(rB.centerOfMass) - transform.forward * dragOffset);
    }

    public void Launch(float velocity)
    {
        transform.parent = null;
        rB.isKinematic = false;
        rB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rB.linearVelocity = transform.forward * velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Collider>().enabled = false;
        transform.parent = collision.transform;
        rB.isKinematic = true;
    }
}

