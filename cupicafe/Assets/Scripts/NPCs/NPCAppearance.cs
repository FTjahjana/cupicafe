using UnityEngine; using UnityEngine.UI;

public class NPCAppearance : MonoBehaviour
{
    public bool randomize; // should usually be on.

    public Renderer[] renderers;

    [Header("Head Stuff")]
    public MeshFilter headMesh; public Mesh[] headOptions; 
    public SpriteRenderer face; public Sprite[] faceSprites;

    [Header("Custom")]
    public int chosenHeadId, chosenfaceId; public Color chosenColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (randomize)
        {
            headMesh.mesh = headOptions[Random.Range(0, headOptions.Length)];
            face.sprite = faceSprites[Random.Range(0, faceSprites.Length)];

            GenerateandApplyColor();
        }
        else
        {
            headMesh.mesh = headOptions[chosenHeadId];
            face.sprite = faceSprites[chosenfaceId];
            ApplyColor(chosenColor);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ApplyColor(Color c)
    {
        foreach (var r in renderers)
            r.material.SetColor("_color", c);
    }

    public void GenerateandApplyColor()
    {
    float h = Random.value;           
    float s = Random.Range(0.5f, 1f);   
    float v = Random.Range(0.5f, 0.9f); 
    Color c = Color.HSVToRGB(h, s, v);  
    
    ApplyColor(c);
    }
}
