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
        Container.Bind<Player>().FromInstance(player).AsSingle();

        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();
        Container.Bind<AdsManager>().FromInstance(adsManager).AsSingle();
        Container.Bind<LanguageManager>().FromInstance(languageManager).AsSingle();
        Container.Bind<AudioManager>().FromInstance(audioManager).AsSingle();

        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle();

        Container.Bind<UIGameMain>().FromInstance(uiGameMain).AsSingle();
        Container.Bind<UIInGame>().FromInstance(uiInGame).AsSingle();
        Container.Bind<UIGameFinish>().FromInstance(uiGameFinish).AsSingle();
    }
}
