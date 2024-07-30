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
        if (other.TryGetComponent<IInteractable>(out IInteractable iInteractable))
            HandleInteractable(iInteractable, iInteractable.GetInfluenceValue());

        else if (other.TryGetComponent<ITriggerable>(out ITriggerable iTriggerable))
            HandleTriggerable(iTriggerable, iTriggerable.GetTriggerableType());
    }

    private void HandleInteractable(IInteractable iInteractable, int influence)
    {
        _gameManager.ChangeMoneyAmount(influence);
        iInteractable.OnInteracted();

        if (influence > 0) GoodCollectEffect.Play();
        else BadCollectEffect.Play();
    }

    private void HandleTriggerable(ITriggerable iTriggerable, TriggerableTypeEnum triggerableType)
    {
        switch (triggerableType)
        {
            case TriggerableTypeEnum.Finish:
                bool finishReached = _gameManager.FinishReached((iTriggerable as TriggerableFinish).GetFinishIndex());

                if (finishReached) iTriggerable.OnTriggered();
                break;
            default: iTriggerable.OnTriggered(); break;
        }
    }
}
