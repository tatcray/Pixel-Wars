using System;
using System.Collections;
using System.Collections.Generic;
using Dependencies;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AdController
{
    private static AdController instance;
    private ServicesDependencies servicesDependencies;
    private Banner banner;
    private Rewarded rewarded;
    private Interstitial interstitial;

    public AdController(ServicesDependencies servicesDependencies)
    {
        instance = this;
        
        this.servicesDependencies = servicesDependencies;
    }

    public static void ShowRewarded(Action onSuccessed, Action onFailed) =>
        instance.rewarded.ShowAd(onSuccessed, onFailed);

    public static void ShowInterstitial(Action onWatch) =>
        instance.interstitial.ShowAd(onWatch, onWatch);

    public void InitializeSdk()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (s) => 
        {
            PreloadAds();
            // ShowBanner();
        };

        MaxSdk.SetSdkKey(servicesDependencies.sdkKey);
        MaxSdk.InitializeSdk();
    }

    private void ShowBanner()
    {
        banner.Show();
    }

    private void PreloadAds()
    {
        banner = new Banner(servicesDependencies.bannerId);
        interstitial = new Interstitial(servicesDependencies.interstitialId);
        rewarded = new Rewarded(servicesDependencies.rewardId);
    }
}

public class Banner
{
    private string bannerAdUnitId;
    public Banner(string adUnitId)
    {
        bannerAdUnitId = adUnitId;
        
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);
    }

    public void Show()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
}

public class Rewarded
{
    public Action rewardWatchedSuccesful;
    public Action rewardViewFailed;
    
    private string adUnitId;
    private int retryAttempt;
    private bool shouldShowOnLoad;

    public Rewarded(string adUnitId)
    {
        this.adUnitId = adUnitId;
        
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (s, info) => RewardFailed();
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (s, info) => RewardFailed();
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (s, info, arg3) => RewardFailed();
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (s, reward, arg3) => RewardSuccess();
        
        LoadRewardedAd();
    }

    public void ShowAd(Action onSuccessed, Action onFailed)
    {
        rewardViewFailed = onFailed;
        rewardWatchedSuccesful = onSuccessed;
        
        if (!ShowRewarded())
        {
            shouldShowOnLoad = true;
            LoadRewardedAd();
        }
    }

    private void RewardFailed()
    {
        rewardViewFailed();
    }

    private void RewardSuccess()
    {
        rewardWatchedSuccesful();
    }

    private bool ShowRewarded()
    {
        if (MaxSdk.IsRewardedAdReady(adUnitId))
        {
            MaxSdk.ShowRewardedAd(adUnitId);
            
            shouldShowOnLoad = false;
            LoadRewardedAd();
            
            return true;
        }

        return false;
    }
    
    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adUnitId);
    }
    
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        retryAttempt = 0;
        
        if (shouldShowOnLoad)
            ShowRewarded();
    }
}

public class Interstitial
{
    private Action viewSuccesed;
    private Action viewFailed;
    
    private string adUnitId;
    private int retryAttempt;
    private bool shouldShowOnLoad;

    public Interstitial(string adUnitId)
    {
        this.adUnitId = adUnitId;
        
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialAdLoadedEvent;
        
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (s, info) => ViewFail();
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (s, info) => ViewSuccesful();
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (s, info, arg3) => ViewFail();
        
        LoadRewardedAd();
    }

    public void ShowAd(Action onSuccessed, Action onFailed)
    {
        this.viewFailed = onFailed;
        this.viewSuccesed = onSuccessed;
        if (!ShowRewarded())
        {
            shouldShowOnLoad = true;
            LoadRewardedAd();
        }
    }

    private void ViewFail()
    {
        viewFailed?.Invoke();
    }

    private void ViewSuccesful()
    {
        viewSuccesed?.Invoke();
    }

    private bool ShowRewarded()
    {
        if (MaxSdk.IsInterstitialReady(adUnitId))
        {
            MaxSdk.ShowInterstitial(adUnitId);
            
            shouldShowOnLoad = false;
            LoadRewardedAd();
            
            return true;
        }

        return false;
    }
    
    private void LoadRewardedAd()
    {
        MaxSdk.LoadInterstitial(adUnitId);
    }
    
    private void OnInterstitialAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        retryAttempt = 0;
        
        if (shouldShowOnLoad)
            ShowRewarded();
    }
}