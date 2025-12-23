using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float airResistance = 1.8f;
    [SerializeField] private float length = 0.7f;
    [SerializeField] private float scalingFactor = 0.001f;

    private Rigidbody rb;
    private float dragOffset;

    public Rigidbody RB => rb;
    public float Length => length;

    private float mapRadius; private Vector3 mapCenter;
    public Hearts hearts;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("missing rigidbody");
        else
            rb.isKinematic = true;
        dragOffset = length * 0.4f;

        mapRadius = GameManager.Instance.mapRadius;
        mapCenter = GameManager.Instance.mapCenter;
    }

    void FixedUpdate()
    {
        float vel = rb.linearVelocity.magnitude; //gets arroe speed
        float drag = airResistance * vel * vel * scalingFactor; // uhh i tried following quadratic drag
        rb.AddForceAtPosition(-rb.linearVelocity.normalized //(the v^ is basiclaly v(normalized)
        * drag, // now we complete the formula by multiplying 
            rb.transform.TransformPoint(rb.centerOfMass) - transform.forward * dragOffset);
        // set drag force slightly behind center of mass so that arrow rotate toward its velocity


        if (!rb.isKinematic)
        {
            if ((transform.position - mapCenter).sqrMagnitude > mapRadius * mapRadius)Destroy(gameObject);
        }
    }

    public void Launch(float velocity)
    {
        if (rb == null)
        {
            Debug.LogError("rb null");
            return;
        }

        transform.parent = null;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rb.linearVelocity = Camera.main.transform.forward * velocity;
        rb.linearVelocity = transform.forward * velocity;
    }

    void OnCollisionEnter(Collision collisionData)
    {
        GetComponent<Collider>().enabled = false;
        transform.parent = collisionData.transform;
        rb.isKinematic = true;

        if (collisionData.collider.CompareTag("NPC"))
        {
            GameObject shot = collisionData.gameObject;
            bool npcshootable = shot.GetComponent<NPCInteract>().shootable;

            if (hearts.CanShoot(shot)&& npcshootable)
            {
                Debug.Log($"{shot.name}: I've been shot!");
                hearts.Shoot(shot);
            }
            else
            {
                Debug.Log($"{shot.name}: Unable to Shoot.");
                return;
            }
        }

    }
}

