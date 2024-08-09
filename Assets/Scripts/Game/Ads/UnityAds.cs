using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace AdsSystem
{
    public class UnityAds : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
#if UNITY_IOS
        private const string _interstitialVideo = "Interstitial_iOS";
        private const string _rewardedVideo = "Rewarded_iOS";
        private string _gameId = "5671246";
#else
        private const string _interstitialVideo = "Interstitial_Android";
        private const string _rewardedVideo = "Rewarded_Android";
        private string _gameId = "5671247";
#endif

        public event Action OnRewardAdCompleted;

        public UnityAds()
        {
            Advertisement.Initialize(_gameId, true, this);
        }

        public void ShowAd()
        {
            Advertisement.Show(_interstitialVideo, this);
        }
        public void ShowRewardedAd()
        {
            Advertisement.Show(_rewardedVideo, this);
        }


        #region Unity Ads Interface Implementations
        public void OnInitializationComplete()
        {
            Advertisement.Load(_rewardedVideo, this);
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Init Failed: [{error}]: {message}");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Load Failed: [{error}:{placementId}] {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"OnUnityAdsShowFailure: [{error}]: {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED && placementId == _rewardedVideo) 
                OnRewardAdCompleted?.Invoke(); 
        }
        #endregion

    }
}