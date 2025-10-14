using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Fruit Data", menuName = "Fruit/Fruit Data")]
public class FruitData : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite icon;
    public Color color = Color.white;
    public float weight = 1f;

    [Header("Stats")]
    [Range(0f, 100f)] public float emotionalProfile;
    public float projectileSpeed;
    public float splashRadius;
    public bool blendable;

    [Header("Reactions")]
    public List<string> compatible_with = new List<string>();
    public List<string> conflicts_with = new List<string>();
}
