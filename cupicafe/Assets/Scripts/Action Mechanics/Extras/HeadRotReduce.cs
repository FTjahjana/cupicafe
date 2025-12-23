using UnityEngine;

public class HeadRotReduce : MonoBehaviour
{
    public bool enableRotLag = true; public bool debug = false;
    [Range(0, 100)] public float lag = 50f;

    void Start(){if (!enableRotLag) {  transform.localRotation = Quaternion.identity; }}
    void LateUpdate()
    {
        if (!enableRotLag) return;

        float parentX = transform.parent.localEulerAngles.x;
        if (parentX > 180f) parentX -= 360f;
        
        float targetX = parentX * (lag / 100f);

        transform.localRotation = Quaternion.Euler(targetX, 0, 0);

        if (debug) Debug.Log($"ParentX: {parentX} | TargetX: {targetX} | locRot: {transform.localEulerAngles.x}");
    }

}