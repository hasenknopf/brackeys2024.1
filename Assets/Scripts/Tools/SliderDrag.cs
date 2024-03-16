using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SliderDrag : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        GameEvents.Instance.Confirm.Invoke();
    }
}