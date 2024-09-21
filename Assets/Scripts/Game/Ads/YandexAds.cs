#if UNITY_WEBGL
using System;
using YG;
using UnityEngine;

namespace AdsSystem
{
    public class YandexAds : IAdsService
    {
        public event Action OnRewardAdCompleted;
        public event Action OnAdCompleted;

        public YandexAds()
        {
            YandexGame.RewardVideoEvent += OnRewardAdClosed;
            YandexGame.CloseFullAdEvent += OnAdCompleted;
        }
        ~YandexAds()
        {
            YandexGame.RewardVideoEvent -= OnRewardAdClosed;
            YandexGame.CloseFullAdEvent -= OnAdCompleted;
        }
         
        public void ShowAd()
        {
            YandexGame.FullscreenShow();
        }

        public void ShowRewardedAd()
        {
            YandexGame.RewVideoShow(1);
        }

        private void OnRewardAdClosed(int i)
        {
            OnRewardAdCompleted?.Invoke();
        }
    }
}
#endif
