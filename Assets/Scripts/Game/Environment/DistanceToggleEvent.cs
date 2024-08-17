using UnityEngine;
using UnityEngine.Events;

namespace DistanceToggle
{
    public class DistanceToggleEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnterEvent;
        [SerializeField] private UnityEvent onTriggerExitEvent;

        public virtual void InvokeEnter() => onTriggerEnterEvent?.Invoke();
        public virtual void InvokeExit() => onTriggerExitEvent?.Invoke();
    }
}
