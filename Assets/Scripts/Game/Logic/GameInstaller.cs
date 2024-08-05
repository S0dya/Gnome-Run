using Zenject;
using UnityEngine;

using Saving;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Player player;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIInGame uiInGame;
    [SerializeField] private UIGameMain uiGameMain;
    [SerializeField] private UIGameFinish uiGameFinish;
    [SerializeField] private PlayerAnimator playerAnimator;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();
        Container.Bind<PlayerMovement>().FromInstance(playerMovement).AsSingle().NonLazy();
        Container.Bind<PlayerAnimator>().FromInstance(playerAnimator).AsSingle().NonLazy();

        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle().NonLazy();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
        Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle().NonLazy();

        Container.Bind<UIGameMain>().FromInstance(uiGameMain).AsSingle().NonLazy();
        Container.Bind<UIInGame>().FromInstance(uiInGame).AsSingle().NonLazy();
        Container.Bind<UIGameFinish>().FromInstance(uiGameFinish).AsSingle().NonLazy();
    }
}
