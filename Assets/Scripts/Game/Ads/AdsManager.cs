using System;
using UnityEngine;
using YG;

namespace AdsSystem
{
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] private YandexGame yandexSdk;

        private IAdsService _adsService;

        private Action _rewardAction;

        public void Init()
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_TVOS
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