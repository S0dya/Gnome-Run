using System;
using System.Collections;
using UnityEngine;

public interface IInteractableStopping : IInteractable
{
    public event Action OnStopInteraction;
}

public class InteractableStopping : Interactable, IInteractableStopping
{
    [SerializeField] private float interactionStopDuration = 1;

    public event Action OnStopInteraction;

    private bool _isInteracted;

    new public virtual void OnInteracted()
    {
        if (_isInteracted) return;
        _isInteracted = true;

        StartCoroutine(InteractionStopCoroutine());
    }

    public void StopInteraction()
    {
        OnStopInteraction?.Invoke();
    }

    private IEnumerator InteractionStopCoroutine()
    {
        yield return new WaitForSeconds(interactionStopDuration);

        StopInteraction();
    }
}