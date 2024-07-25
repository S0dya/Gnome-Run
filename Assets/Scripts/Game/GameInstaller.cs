using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIInGame uiInGame;
    [SerializeField] private UIGameMain uiGameMain;
    [SerializeField] private UIGameFinish uiGameFinish;
    [SerializeField] private PlayerAnimator playerAnimator;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
        Container.Bind<UIGameMain>().FromInstance(uiGameMain).AsSingle().NonLazy();
        Container.Bind<UIInGame>().FromInstance(uiInGame).AsSingle().NonLazy();
        Container.Bind<UIGameFinish>().FromInstance(uiGameFinish).AsSingle().NonLazy();
        Container.Bind<PlayerAnimator>().FromInstance(playerAnimator).AsSingle().NonLazy();
    }
}
