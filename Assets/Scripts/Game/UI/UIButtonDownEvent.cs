using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonDownEvent : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UnityEvent eventToInvoke;

    public void OnPointerDown(PointerEventData eventData) => eventToInvoke.Invoke();
}
