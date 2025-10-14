using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = null;
    [SerializeField] private float launchVelocity = 40f;

    private GameObject spawnPoint = null;

    void Start()
    {
        spawnPoint = transform.Find("DrawPoint").gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireArrow();
        }
    }

    void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.Launch(launchVelocity);
    }
}

