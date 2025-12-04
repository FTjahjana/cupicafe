using UnityEngine;

public class NPCAppearance : MonoBehaviour
{
    public bool randomize = true;

    public Renderer[] renderers;

    [Header("Head Stuff")]
    public int headIndex = 0;
    public MeshFilter headMesh; public Mesh[] headOptions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (randomize)
        {
            headIndex = Random.Range(0, headOptions.Length);
        }

        headMesh.mesh = headOptions[headIndex];
        ApplyColor(new Color(Random.value, Random.value, Random.value));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ApplyColor(Color c)
    {
        foreach (var r in renderers)
            r.material.color = c;
    }
}
