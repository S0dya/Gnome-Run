using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerTrigger : MonoBehaviour
{

    private GameManager _gameManager;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _gameManager.ChangeMoneyAmount(interactable.GetInfluenceValue());
            interactable.OnInteracted();

            return;
        }

        var triggerableFinish = other.GetComponent<ITriggerableFinish>();
        if (triggerableFinish != null)
        {
            bool finishReached = _gameManager.FinishReached(triggerableFinish.GetFinishIndex());

            if (finishReached) triggerableFinish.OnTriggered();
            return;
        }

        var triggerable = other.GetComponent<ITriggerable>();
        if (triggerable != null)
        {
            triggerable.OnTriggered();
        }
    }
}
