using UnityEngine;

public enum TriggerableTypeEnum
{
    none,
    Finish,
    Rotate,

}

public interface ITriggerable
{
    public TriggerableTypeEnum GetTriggerableType();
    public void OnTriggered();
}

public class Triggerable : MonoBehaviour, ITriggerable
{
    [Header("Settings")]
    [SerializeField] private bool playsAnimation = true;
    [SerializeField] private string animationName;

    [SerializeField] private TriggerableTypeEnum triggerableType;
    [SerializeField] private Animator animator;

    private int _aniamtorIDAnimationName;

    private void Start()
    {
        _aniamtorIDAnimationName = Animator.StringToHash(animationName);
    }

    public TriggerableTypeEnum GetTriggerableType() => triggerableType;

    public void OnTriggered()
    {
        if (playsAnimation) animator.Play(_aniamtorIDAnimationName);
    }
}
