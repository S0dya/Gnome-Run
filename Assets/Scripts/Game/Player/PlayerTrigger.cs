using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerTrigger : SubjectMonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] ParticleSystem GoodCollectEffect;
    [SerializeField] ParticleSystem BadCollectEffect;

    private GameManager _gameManager;

    private bool _inLevel;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Awake()
    {
        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevel},
            { EventEnum.LevelFinishedGameover, OnFinishLevel},
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable iInteractable))
            HandleInteractable(iInteractable, iInteractable.GetInfluenceValue());

        else if (other.TryGetComponent<ITriggerable>(out ITriggerable iTriggerable))
            HandleTriggerable(iTriggerable, iTriggerable.GetTriggerableType());
    }

    private void HandleInteractable(IInteractable iInteractable, int influence)
    {
        _gameManager.ChangeMoneyAmount(influence);
        iInteractable.OnInteracted();

        if (iInteractable is IInteractablePatrol iInteractablePatrol)
        {
            iInteractablePatrol.OnInteracted(transform.position);
        }
        if (iInteractable is IInteractableStopping iInteractableStopping)
        {
            iInteractableStopping.OnStopInteraction += MovePlayer;

            if (influence > 0) player.GoodInteractionStopMovement();
            else player.BadInteractionStopMovement();
        }

        if (influence > 0) GoodCollectEffect.Play();
        else BadCollectEffect.Play();
    }

    private void HandleTriggerable(ITriggerable iTriggerable, TriggerableTypeEnum triggerableType)
    {
        switch (triggerableType)
        {
            case TriggerableTypeEnum.Finish:
                bool finishReached = _gameManager.FinishReached((iTriggerable as ITriggerableFinish).GetFinishIndex());

                if (!finishReached) iTriggerable.OnTriggered();
                break;
            default: iTriggerable.OnTriggered(); break;
        }
    }

    //actions
    private void MovePlayer()
    {
        if (_inLevel) player.InteractionStopMovementStopped();
    }

    private void OnStartLevel()
    {
        _inLevel = true;
    }
    private void OnFinishLevel()
    {
        _inLevel = false;
    }
}
