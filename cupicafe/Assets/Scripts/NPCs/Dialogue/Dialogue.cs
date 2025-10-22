using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string name; 
    
    [TextArea(3, 8)]
    public string[] sentences;
}