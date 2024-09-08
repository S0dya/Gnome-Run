using System;

namespace AdsSystem
{
    public interface IAdsService
    {
        public event Action OnRewardAdCompleted;
        public event Action OnAdCompleted;

        public void ShowAd();
        public void ShowRewardedAd();
    }
}
