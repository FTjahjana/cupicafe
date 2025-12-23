using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover start!");
        // Add hover effects here, e.g., change color, play sound, etc.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hover end!");
    }
}
