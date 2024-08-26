#if UNITY_WEBGL
using System;
using YG;

namespace AdsSystem
{
    public class YandexAds : IAdsService
    {
        public event Action OnRewardAdCompleted;

        public YandexAds(YandexGame sdk)
        {
            YandexGame.RewardVideoEvent += OnRewardAdClosed;
        }
        ~YandexAds()
        {
            YandexGame.RewardVideoEvent -= OnRewardAdClosed;
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
