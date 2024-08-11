using Zenject;
using UnityEngine;

using Saving;
using AdsSystem;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;

    [SerializeField] private SaveManager saveManager;
    [SerializeField] private AdsManager adsManager;
    [SerializeField] private LanguageManager languageManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private UIInGame uiInGame;
    [SerializeField] private UIGameMain uiGameMain;
    [SerializeField] private UIGameFinish uiGameFinish;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle().NonLazy();

        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle().NonLazy();
        Container.Bind<AdsManager>().FromInstance(adsManager).AsSingle().NonLazy();
        Container.Bind<LanguageManager>().FromInstance(languageManager).AsSingle().NonLazy();
        Container.Bind<AudioManager>().FromInstance(audioManager).AsSingle().NonLazy();

        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle().NonLazy();
        Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle().NonLazy();

        Container.Bind<UIGameMain>().FromInstance(uiGameMain).AsSingle().NonLazy();
        Container.Bind<UIInGame>().FromInstance(uiInGame).AsSingle().NonLazy();
        Container.Bind<UIGameFinish>().FromInstance(uiGameFinish).AsSingle().NonLazy();
    }
}
