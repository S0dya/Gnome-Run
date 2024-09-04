using UnityEngine;
using Zenject;
using YG;

using Saving;
using AdsSystem;

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
#if UNITY_WEBGL
        YandexGame.GetDataEvent += OnCloudSaveLoaded;
#else
        InitializeGame();
#endif
    }

    private void OnCloudSaveLoaded()
    {
        YandexGame.GetDataEvent -= OnCloudSaveLoaded;

        InitializeGame();
    }

    private void InitializeGame()
    {
        _container.Resolve<SaveManager>().Init();
        _container.Resolve<AdsManager>().Init();

        _container.Resolve<Player>().Init();
        _container.Resolve<LevelManager>().Init();
        _container.Resolve<UIGameMain>().Init();
        _container.Resolve<AudioManager>().Init();
    }
}
