using System;
using YG;

namespace AdsSystem
{
    public class YandexAds : IAdsService
    {
        public event Action OnRewardAdCompleted;

        private readonly YandexGame _sdk;

        public YandexAds(YandexGame sdk)
        {
            _sdk = sdk;

            YandexGame.CloseVideoEvent += OnRewardAdClosed;
        }
         
        public void ShowAd()
        {
            _sdk._FullscreenShow();
        }

        public void ShowRewardedAd()
        {
            _sdk._RewardedShow(1);
        }

        private void OnRewardAdClosed()
        {
            OnRewardAdCompleted?.Invoke();
        }

        ~YandexAds()
        {
            // Unsubscribe to avoid memory leaks
            YandexGame.CloseVideoEvent -= OnRewardAdClosed;
        }
    }
}
