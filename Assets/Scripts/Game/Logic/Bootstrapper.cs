using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PlayerAnimator playerAnimator;
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    private void Awake()
    {
        LevelManager.Default.Init();
        _container.Resolve<GameManager>().Init();
        _container.Resolve<UIGameMain>().Init();
        _container.Resolve<UIInGame>().Init();
        _container.Resolve<UIGameFinish>().Init();

        playerAnimator.Init();
    }
}
