using System;

namespace AdsSystem
{
    public interface IAdsService
    {
        public event Action OnRewardAdCompleted;

        public void ShowAd();
        public void ShowRewardedAd();
    }
}
