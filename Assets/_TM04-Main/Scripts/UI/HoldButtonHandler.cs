using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoldButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPointerDown = false;
    public UnityEvent OnButtonHold;

    void Update()
    {
        if (isPointerDown)
        {
            OnButtonHold?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }
}