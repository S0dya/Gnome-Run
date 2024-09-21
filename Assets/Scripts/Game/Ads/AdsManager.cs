using System;
using UnityEngine;
#if UNITY_WEBGL
using YG;
#endif

namespace AdsSystem
{
    public class AdsManager : MonoBehaviour
    {
        private IAdsService _adsService;

        private Action _rewardAction;
        private Action _adClosedAction;

        private bool _takesBreakFromAd;

        public void Init()
        {
#if UNITY_WEBGL
            if (Settings.CurrentPlatformType == Settings.PlatformType.Yandex)
                _adsService = new YandexAds();
#else
            _adsService = new UnityAds();
#endif

            if (_adsService == null) return;
            _adsService.OnRewardAdCompleted += OnRewardAdCompleted;
            _adsService.OnAdCompleted += _adClosedAction;
        }

        public void ShowAd()
        {
            if (_takesBreakFromAd)
            {
                _takesBreakFromAd = false;
                return;
            }

            Observer.OnHandleEvent(EventEnum.AdOpened); 

            _adsService?.ShowAd();
        }
        public void ShowRewardAd(Action action)
        {
            Observer.OnHandleEvent(EventEnum.AdOpened);

            _rewardAction = action;

            _adsService?.ShowRewardedAd();
        }

        private void OnRewardAdCompleted()
        {
            _takesBreakFromAd = true;

            _rewardAction?.Invoke();

            OnAdCompleted();
        }
        private void OnAdCompleted() => Observer.OnHandleEvent(EventEnum.AdClosed);
    }
}