using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NPCIconCamera : MonoBehaviour
{
    public Camera iconCam;
    public Vector3 offset; public RenderTexture placeholderRT;
    public Transform ExtraCamerasParentGroup;

    public RenderTexture CaptureIcon(GameObject npc)
    {
        var rt = new RenderTexture(256, 256, 16);

        iconCam.transform.SetParent(npc.transform);
        iconCam.transform.localPosition = offset;
        iconCam.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

        iconCam.targetTexture = rt;
        iconCam.Render();
        iconCam.targetTexture = placeholderRT;

        iconCam.transform.SetParent(ExtraCamerasParentGroup);
        this.gameObject.SetActive(false);
        return rt;
        
    }
}