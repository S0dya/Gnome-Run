using Zenject;
using UnityEngine;
using ButchersGames;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIInGame uiInGame;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
        Container.Bind<UIInGame>().FromInstance(uiInGame).AsSingle().NonLazy();
    }
}
