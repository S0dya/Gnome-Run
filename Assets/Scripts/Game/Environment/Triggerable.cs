using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Triggered");
    }
}
