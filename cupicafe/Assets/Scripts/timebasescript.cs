using UnityEngine;
using System.Collections;         
using System.Collections.Generic; 


public class timebasescript : MonoBehaviour
{
    public float startTime = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startTime -= Time.deltaTime;
        Debug.Log("time: " + startTime);
    }

    IEnumerator timeExample()
    {
    Debug.Log("Coroutine started at: " + Time.time);
    yield return new WaitForSeconds(1); 
    Debug.Log("Coroutine resumed at: " + Time.time);
    }

    void showPlayerScore(){

    }
}
