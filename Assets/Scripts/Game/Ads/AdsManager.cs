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

        public void Init()
        {
#if UNITY_WEBGL
            _adsService = new YandexAds(yandexSdk);
#else
            _adsService = new UnityAds();
#endif

            _adsService.OnRewardAdCompleted += OnRewardAdCompleted;
        }

        public void ShowAd() => _adsService.ShowAd();
        public void ShowRewardAd(Action action)
        {
            _rewardAction = action;

            _adsService.ShowRewardedAd();
        }

        private void OnRewardAdCompleted() => _rewardAction?.Invoke();
    }
}