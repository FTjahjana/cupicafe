using UnityEngine;
using System.Collections;         
using System.Collections.Generic; 

public class npcint2d : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Debug.Log($"Interact with {other.gameObject.name}?");
        }
    }
}
