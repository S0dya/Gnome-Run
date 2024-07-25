using UnityEngine;
using Zenject;

public class PlayerTrigger : MonoBehaviour
{

    [SerializeField] ParticleSystem GoodCollectEffect;
    [SerializeField] ParticleSystem BadCollectEffect;

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
            var influence = interactable.GetInfluenceValue();

            _gameManager.ChangeMoneyAmount(influence);
            interactable.OnInteracted();

            if (influence > 0) GoodCollectEffect.Play();
            else BadCollectEffect.Play();

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
