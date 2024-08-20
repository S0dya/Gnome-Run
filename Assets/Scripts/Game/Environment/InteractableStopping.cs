using System;
using System.Collections;
using UnityEngine;

public interface IInteractableStopping : IInteractable
{
    public event Action OnStopInteraction;
}

public class InteractableStopping : Interactable, IInteractableStopping
{
    [Header("Settings")]
    [SerializeField] private float interactionStopDuration = 1;

    [Header("Other")]
    [SerializeField] private BoxCollider interactionCollider;

    public event Action OnStopInteraction;

    public override void OnInteracted()
    {
        interactionCollider.enabled = false;

        StartInteractionStop();
    }

    public void StopInteraction()
    {
        OnStopInteraction?.Invoke();
    }

    private protected void StartInteractionStop() => StartCoroutine(InteractionStopCoroutine());
    private IEnumerator InteractionStopCoroutine()
    {
        yield return new WaitForSeconds(interactionStopDuration);

        StopInteraction();
    }
}