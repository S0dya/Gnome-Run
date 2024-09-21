using UnityEngine;
using Zenject;
using YG;

using Saving;
using AdsSystem;
using System.Collections;

public class Bootstrapper : MonoBehaviour
{
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    private void Awake()
    {
        _container.Resolve<SaveManager>().Init();

        if (Settings.CurrentPlatformType == Settings.PlatformType.Yandex)
            YandexGame.GetDataEvent += OnCloudSaveLoaded;
        else 
            InitializeGame();
    }

    private void OnCloudSaveLoaded()
    {
        YandexGame.GetDataEvent -= OnCloudSaveLoaded;

        StartCoroutine(InitializeGameWithDelay());
    }
    private IEnumerator InitializeGameWithDelay()
    {
        yield return null;

        InitializeGame();
    }

    private void InitializeGame()
    {
        _container.Resolve<LanguageManager>().Init();
        _container.Resolve<AdsManager>().Init();

        _container.Resolve<Player>().Init();
        _container.Resolve<LevelManager>().Init();
        _container.Resolve<UIGameMain>().Init();
        _container.Resolve<AudioManager>().Init();
    }
}
