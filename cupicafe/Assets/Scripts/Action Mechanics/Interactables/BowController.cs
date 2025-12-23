using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BowController : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab = null;
    [SerializeField] private float launchVelocity = 40f;

    private GameObject spawnPoint = null;

    [SerializeField] InputActionReference fireAction;
    [SerializeField] Hearts hearts;

    void Start()
    {
        spawnPoint = transform.Find("Arrow Spawn").gameObject;
    }

    void Update()
    {
        if (fireAction.action.WasPressedThisFrame() && transform.parent == GameManager.Instance.hand)
        {
            FireArrow();
        }
    }

    void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        if (arrowController == null){ Debug.LogError("missing ArrowController script"); return;}
        arrowController.hearts = hearts;

        arrowController.Launch(launchVelocity);
    }
    
        public void ClearArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject arrow in arrows) Destroy(arrow);
    }
}

