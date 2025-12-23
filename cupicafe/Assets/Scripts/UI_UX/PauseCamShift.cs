using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseCamShift : MonoBehaviour
{
public GameObject playerCamera, player;
Transform PsavedLoc, CsavedLoc;

public void CamShift(bool pause)
{
    if (pause)
    {
        player = GameManager.Instance.Player;
        player.GetComponent<PlayerMovement3D>().ToggleActions(false);

        PsavedLoc = player.transform;
        CsavedLoc = playerCamera.transform;
        //StartCoroutine(LerpRotation(PsavedLoc ,PsavedLoc.localRotation, Quaternion.identity, 0.15f));
        StartCoroutine(LerpRotation(CsavedLoc ,CsavedLoc.localRotation, Quaternion.identity, 0.15f));
        //StartCoroutine(LerpPosition(playerCamera.transform ,CsavedLoc.localPosition, new Vector3(0,0,0), 0.15f));
        GameManager.Instance.Player.GetComponent<Animator>().SetTrigger("Pause");
    }
    else
    {
        //StartCoroutine(LerpRotation(playerCamera.transform ,Quaternion.identity, CsavedLoc.localRotation, 0.15f));
        GameManager.Instance.Player.GetComponent<Animator>().SetTrigger("Resume");
    }
}

IEnumerator LerpRotation(Transform target, Quaternion startLoc, Quaternion targetLoc, float duration)
{
    Debug.Log("Lerping rotation from "+startLoc +"to "+targetLoc);
    float t = 0f;

    while (t < 1f)
    {
        t += Time.deltaTime / duration;
        target.localRotation = Quaternion.Slerp(startLoc, targetLoc, t);
        yield return null;
    }
}

IEnumerator LerpPosition(Transform target, Vector3 startLoc, Vector3 targetLoc, float duration)
{
    Debug.Log("Lerping rotation from "+startLoc +"to "+targetLoc);
    float t = 0f;

    while (t < 1f)
    {
        t += Time.deltaTime / duration;
        target.localPosition = Vector3.Lerp(startLoc, targetLoc, t);
        yield return null;
    }
}

}
