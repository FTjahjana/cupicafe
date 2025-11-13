using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SOE : MonoBehaviour
{ // tag this gameobj lol
    int SOEindex;

    public void IncSOE()
    { SOEindex = GameManager.Instance.SOEindex;
        Debug.Log("SOEindex is now"+ SOEindex);
    }
    
}