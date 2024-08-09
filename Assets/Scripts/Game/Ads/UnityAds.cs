using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace AdsSystem
{
    public class UnityAds : IAdsService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
#if UNITY_IOS
    const string rewardedVideo = "Rewarded_iOS";
    string gameId = "5671246";
#else
        const string rewardedVideo = "Rewarded_Android";
        string gameId = "5671247";
#endif

        public UnityAds()
        {
            Advertisement.Initialize(gameId, true, this);
        }

        public void LoadRewardedAd()
        {
            Advertisement.Load(rewardedVideo, this);
        }

        public void ShowAd()
        {

        }
        public void ShowRewardedAd()
        {
            Advertisement.Show(rewardedVideo, this);
        }


        #region Unity Ads Interface Implementations
        public void OnInitializationComplete()
        {
            LoadRewardedAd();
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

        }
        #endregion

    }
}