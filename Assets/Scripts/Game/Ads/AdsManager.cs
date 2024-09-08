using System;
using UnityEngine;
#if UNITY_WEBGL
using YG;
#endif

namespace AdsSystem
{
    public class AdsManager : MonoBehaviour
    {
#if UNITY_WEBGL
        [SerializeField] private YandexGame yandexSdk;
#endif

        private IAdsService _adsService;

        private Action _rewardAction;
        private Action _adClosedAction;

        public void Init()
        {
#if UNITY_WEBGL
            _adsService = new YandexAds(yandexSdk);
#else
            _adsService = new UnityAds();
#endif

            _adsService.OnRewardAdCompleted += OnRewardAdCompleted;
            _adsService.OnAdCompleted += OnAdCompleted;
        }

        private void Start()
        {
            ShowAd();
        }

        public void ShowAd()
        {
            Observer.OnHandleEvent(EventEnum.AdOpened);

            _adsService.ShowAd();
        }
        public void ShowRewardAd(Action action)
        {
            Observer.OnHandleEvent(EventEnum.AdOpened);

            _rewardAction = action;

            _adsService.ShowRewardedAd();
        }

        private void OnRewardAdCompleted()
        {
            _rewardAction?.Invoke();

            OnAdCompleted();
        }
        private void OnAdCompleted() => Observer.OnHandleEvent(EventEnum.AdClosed);
    }
}