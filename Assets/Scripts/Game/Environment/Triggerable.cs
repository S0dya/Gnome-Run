using UnityEngine;

public interface ITriggerable
{
    public void OnTriggered();
}

public class Triggerable : MonoBehaviour, ITriggerable
{
    [SerializeField] private Animator animator;

    public void OnTriggered()
    {
    }
}
