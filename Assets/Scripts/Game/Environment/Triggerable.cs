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
    [SerializeField] TriggerableTypeEnum triggerableType;
    [SerializeField] private Animator animator;

    public TriggerableTypeEnum GetTriggerableType()
    {
        return triggerableType;
    }

    public void OnTriggered()
    {

    }
}
