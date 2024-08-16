using UnityEngine;
using UnityEngine.Events;

public class DistanceToggleEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnterEvent;
    [SerializeField] private UnityEvent onTriggerExitEvent;

    public void InvokeEnter() => onTriggerEnterEvent?.Invoke();
    public void InvokeExit() => onTriggerExitEvent?.Invoke();
}