using UnityEngine;
using UnityEngine.Events;

public class AnimatorAdditionalEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnAnimationEnd;

    public void OnAnimationEnd() => eventOnAnimationEnd.Invoke();
}
