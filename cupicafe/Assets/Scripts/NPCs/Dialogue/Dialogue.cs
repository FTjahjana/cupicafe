using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    [Tooltip("Use \"Input\" here to trigger input Panel instead")]
    public string name; 
    
    [Tooltip("Use \"[Player]\" to replace with player Name")]
    [TextArea(3, 8)] public string[] sentences;
}