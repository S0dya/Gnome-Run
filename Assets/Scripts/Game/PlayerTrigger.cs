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
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _gameManager.ChangeMoneyAmount(interactable.GetInfluenceValue());
            interactable.OnInteracted();

            return;
        }

        switch(other.gameObject.tag)
        {
            case "TriggerableFinish":
                ITriggerableFinish triggerableFinish = other.GetComponent<ITriggerableFinish>();
                bool finishReached = _gameManager.FinishReached(triggerableFinish.GetFinishIndex());

                if (finishReached) triggerableFinish.OnTriggered();
                break;
            case "TriggerableRotate":

                break;
            default:
                var triggerable = other.GetComponent<ITriggerable>();
                triggerable.OnTriggered();
                break;
        }
    }
}
