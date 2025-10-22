using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = null;
    [SerializeField] private float launchVelocity = 40f;

    private GameObject spawnPoint = null;

    void Start()
    {
        spawnPoint = transform.Find("Arrow Spawn").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FireArrow();
        }
    }

    void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        ArrowController arrowController = arrow.GetComponent<ArrowController>();

        if (arrowController == null)
        {
            Debug.LogError("missing ArrowController script");
            return;
        }

        arrowController.Launch(launchVelocity);
    }
    
        public void ClearArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject arrow in arrows) Destroy(arrow);
    }
}

