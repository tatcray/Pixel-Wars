#if UNITY_IPHONE
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AOT;
using UnityEngine;
using ConsentManager;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.iOS
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ShiftExpressionRealShiftCountIsZero")]
    public class AppodealAdsClient : IAppodealAdsClient
    {
        private const int AppodealAdTypeInterstitial = 1 << 0;
        private const int AppodealAdTypeBanner = 1 << 2;
        private const int AppodealAdTypeRewardedVideo = 1 << 4;
        private const int AppodealAdTypeMrec = 1 << 5;

        private const int AppodealShowStyleInterstitial = 1;
        private const int AppodealShowStyleBannerTop = 4;
        private const int AppodealShowStyleBannerBottom = 8;
        private const int AppodealShowStyleRewardedVideo = 16;
        private const int AppodealShowStyleBannerLeft = 1 << 6;
        private const int AppodealShowStyleBannerRight = 1 << 7;

        private static IInAppPurchaseValidationListener inAppPurchaseValidationListener;
        private static IAppodealInitializationListener initializationListener;

        private static IInterstitialAdListener interstitialListener;
        private static IRewardedVideoAdListener rewardedVideoListener;
        private static IBannerAdListener bannerListener;
        private static IMrecAdListener mrecListener;


        #region AppodealInitialization delegate

        [MonoPInvokeCallback(typeof(AppodealInitializationCallback))]
        private static void appodealSdkDidInitialize()
        {
            initializationListener?.onInitializationFinished(null);
        }

        private void setAppodealInitializationCallback(IAppodealInitializationListener listener)
        {
            initializationListener = listener;

            AppodealObjCBridge.AppodealSetInitializationDelegate(appodealSdkDidInitialize);
        }

        #endregion

        #region Interstitial Delegate

        [MonoPInvokeCallback(typeof(AppodealInterstitialDidLoadCallback))]
        private static void interstitialDidLoad(bool isPrecache)
        {
            interstitialListener?.onInterstitialLoaded(isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void interstitialDidFailToLoad()
        {
            interstitialListener?.onInterstitialFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void interstitialDidFailToPresent()
        {
            interstitialListener?.onInterstitialShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void interstitialDidClick()
        {
            interstitialListener?.onInterstitialClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        public static void interstitialDidDismiss()
        {
            interstitialListener?.onInterstitialClosed();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void interstitialWillPresent()
        {
            interstitialListener?.onInterstitialShown();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void interstitialDidExpired()
        {
            interstitialListener?.onInterstitialExpired();
        }

        public void setInterstitialCallbacks(IInterstitialAdListener listener)
        {
            interstitialListener = listener;

            AppodealObjCBridge.AppodealSetInterstitialDelegate(
                interstitialDidLoad,
                interstitialDidFailToLoad,
                interstitialDidFailToPresent,
                interstitialWillPresent,
                interstitialDidDismiss,
                interstitialDidClick,
                interstitialDidExpired
            );
        }

        #endregion

        #region Rewarded Video Delegate

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoDidLoadCallback))]
        private static void rewardedVideoDidLoadAd(bool isPrecache)
        {
            rewardedVideoListener?.onRewardedVideoLoaded(isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void rewardedVideoDidFailToLoadAd()
        {
            rewardedVideoListener?.onRewardedVideoFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void rewardedVideoDidFailToPresentWithError()
        {
            rewardedVideoListener?.onRewardedVideoShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoDidDismissCallback))]
        private static void rewardedVideoWillDismiss(bool isFinished)
        {
            rewardedVideoListener?.onRewardedVideoClosed(isFinished);
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoDidFinishCallback))]
        private static void rewardedVideoDidFinish(double amount, string name)
        {
            rewardedVideoListener?.onRewardedVideoFinished(amount, name);
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void rewardedVideoDidPresent()
        {
            rewardedVideoListener?.onRewardedVideoShown();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void rewardedVideoDidExpired()
        {
            rewardedVideoListener?.onRewardedVideoExpired();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void rewardedVideoDidReceiveTap()
        {
            rewardedVideoListener?.onRewardedVideoClicked();
        }

        public void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            rewardedVideoListener = listener;

            AppodealObjCBridge.AppodealSetRewardedVideoDelegate(
                rewardedVideoDidLoadAd,
                rewardedVideoDidFailToLoadAd,
                rewardedVideoDidFailToPresentWithError,
                rewardedVideoWillDismiss,
                rewardedVideoDidFinish,
                rewardedVideoDidPresent,
                rewardedVideoDidExpired,
                rewardedVideoDidReceiveTap
            );
        }

        #endregion

        #region Banner Delegate

        [MonoPInvokeCallback(typeof(AppodealBannerDidLoadCallback))]
        private static void bannerDidLoadAd(int height, bool isPrecache)
        {
            bannerListener?.onBannerLoaded(height, isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void bannerDidFailToLoadAd()
        {
            bannerListener?.onBannerFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void bannerDidClick()
        {
            bannerListener?.onBannerClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void bannerDidExpired()
        {
            bannerListener?.onBannerExpired();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void bannerDidShow()
        {
            bannerListener?.onBannerShown();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void bannerDidFailToPresent()
        {
            bannerListener?.onBannerShowFailed();
        }


        [MonoPInvokeCallback(typeof(AppodealBannerViewDidLoadCallback))]
        private static void bannerViewDidLoadAd(int height, bool isPrecache)
        {
            bannerListener?.onBannerLoaded(height, isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void bannerViewDidFailToLoadAd()
        {
            bannerListener?.onBannerFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void bannerViewDidClick()
        {
            bannerListener?.onBannerClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void bannerViewDidShow()
        {
            bannerListener?.onBannerShown();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void bannerViewDidFailToPresent()
        {
            bannerListener?.onBannerShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void bannerViewDidExpired()
        {
            bannerListener?.onBannerExpired();
        }

        public void setBannerCallbacks(IBannerAdListener listener)
        {
            bannerListener = listener;

            AppodealObjCBridge.AppodealSetBannerDelegate(
                bannerDidLoadAd,
                bannerDidFailToLoadAd,
                bannerDidClick,
                bannerDidExpired,
                bannerDidShow,
                bannerDidFailToPresent);

            AppodealObjCBridge.AppodealSetBannerViewDelegate(
                bannerViewDidLoadAd,
                bannerViewDidFailToLoadAd,
                bannerViewDidClick,
                bannerViewDidShow,
                bannerViewDidFailToPresent,
                bannerViewDidExpired);
        }

        #endregion

        #region Mrec Delegate

        [MonoPInvokeCallback(typeof(AppodealMrecViewDidLoadCallback))]
        private static void mrecViewDidLoadAd(bool isPrecache)
        {
            mrecListener?.onMrecLoaded(isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void mrecViewDidFailToLoadAd()
        {
            mrecListener?.onMrecFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void mrecViewDidClick()
        {
            mrecListener?.onMrecClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void mrecViewDidShow()
        {
            mrecListener?.onMrecShown();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void mrecViewDidFailToPresent()
        {
            mrecListener?.onMrecShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void mrecViewDidExpired()
        {
            mrecListener?.onMrecExpired();
        }

        public void setMrecCallbacks(IMrecAdListener listener)
        {
            mrecListener = listener;

            AppodealObjCBridge.AppodealSetMrecViewDelegate(
                mrecViewDidLoadAd,
                mrecViewDidFailToLoadAd,
                mrecViewDidClick,
                mrecViewDidShow,
                mrecViewDidFailToPresent,
                mrecViewDidExpired);
        }

        #endregion

        #region In-App Purchase Validation delegate

        [MonoPInvokeCallback(typeof(InAppPurchaseValidationSucceededCallback))]
        private static void inAppPurchaseValidationSucceeded(string json)
        {
            inAppPurchaseValidationListener?.onInAppPurchaseValidationSucceeded(json);
        }

        [MonoPInvokeCallback(typeof(InAppPurchaseValidationFailedCallback))]
        private static void inAppPurchaseValidationFailed(string error)
        {
            inAppPurchaseValidationListener?.onInAppPurchaseValidationFailed(error);
        }

        #endregion

        private static int nativeAdTypesForType(int adTypes)
        {
            var nativeAdTypes = 0;

            if ((adTypes & Appodeal.INTERSTITIAL) > 0)
            {
                nativeAdTypes |= AppodealAdTypeInterstitial;
            }

            if ((adTypes & Appodeal.BANNER) > 0 ||
                (adTypes & Appodeal.BANNER_VIEW) > 0 ||
                (adTypes & Appodeal.BANNER_TOP) > 0 ||
                (adTypes & Appodeal.BANNER_LEFT) > 0 ||
                (adTypes & Appodeal.BANNER_RIGHT) > 0 ||
                (adTypes & Appodeal.BANNER_BOTTOM) > 0)
            {
                nativeAdTypes |= AppodealAdTypeBanner;
            }

            if ((adTypes & Appodeal.MREC) > 0)
            {
                nativeAdTypes |= AppodealAdTypeMrec;
            }

            if ((adTypes & Appodeal.REWARDED_VIDEO) > 0)
            {
                nativeAdTypes |= AppodealAdTypeRewardedVideo;
            }

            return nativeAdTypes;
        }

        private static int nativeShowStyleForType(int adTypes)
        {
            if ((adTypes & Appodeal.INTERSTITIAL) > 0)
            {
                return AppodealShowStyleInterstitial;
            }

            if ((adTypes & Appodeal.BANNER_TOP) > 0)
            {
                return AppodealShowStyleBannerTop;
            }

            if ((adTypes & Appodeal.BANNER_BOTTOM) > 0)
            {
                return AppodealShowStyleBannerBottom;
            }

            if ((adTypes & Appodeal.BANNER_LEFT) > 0)
            {
                return AppodealShowStyleBannerLeft;
            }

            if ((adTypes & Appodeal.BANNER_RIGHT) > 0)
            {
                return AppodealShowStyleBannerRight;
            }

            if ((adTypes & Appodeal.REWARDED_VIDEO) > 0)
            {
                return AppodealShowStyleRewardedVideo;
            }

            return 0;
        }

        private static string dictionaryToString(Dictionary <string, object> dictionary)
        {
            var dictionaryString = dictionary.Aggregate("", (current, keyValues)
                => current + (keyValues.Key + "=" + keyValues.Value.GetType() + ":" + keyValues.Value + ","));
            return dictionaryString.TrimEnd(',');
        }

        public void initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            if (listener != null) setAppodealInitializationCallback(listener);

            AppodealObjCBridge.AppodealInitialize(appKey, nativeAdTypesForType(adTypes),
                Appodeal.getPluginVersion(), Appodeal.getUnityVersion());
        }

        public void initialize(string appKey, int adTypes)
        {
            initialize(appKey, adTypes, true);
        }

        public void initialize(string appKey, int adTypes, bool hasConsent)
        {
            AppodealObjCBridge.AppodealInitializeOld(appKey, nativeAdTypesForType(adTypes), hasConsent,
                Appodeal.getPluginVersion(), Appodeal.getUnityVersion());
        }

        public void initialize(string appKey, int adTypes, Consent consent)
        {
            AppodealObjCBridge.AppodealInitializeWithConsent(appKey, nativeAdTypesForType(adTypes),
                Appodeal.getPluginVersion(), Appodeal.getUnityVersion());
        }

        public bool isInitialized(int adType)
        {
            return AppodealObjCBridge.AppodealIsInitialized(nativeAdTypesForType(adType));
        }

        public bool show(int adTypes)
        {
            return AppodealObjCBridge.AppodealShowAd(nativeShowStyleForType(adTypes));
        }

        public bool show(int adTypes, string placement)
        {
            return AppodealObjCBridge.AppodealShowAdforPlacement(nativeShowStyleForType(adTypes), placement);
        }

        public bool showBannerView(int YAxis, int XGravity, string Placement)
        {
            return AppodealObjCBridge.AppodealShowBannerAdViewforPlacement(YAxis, XGravity, Placement);
        }

        public bool showMrecView(int YAxis, int XGravity, string Placement)
        {
            return AppodealObjCBridge.AppodealShowMrecAdViewforPlacement(YAxis, XGravity, Placement);
        }

        public bool isLoaded(int adTypes)
        {
            return AppodealObjCBridge.AppodealIsReadyWithStyle(nativeShowStyleForType(adTypes));
        }

        public void cache(int adTypes)
        {
            AppodealObjCBridge.AppodealCacheAd(nativeAdTypesForType(adTypes));
        }

        public void setAutoCache(int adTypes, bool autoCache)
        {
            AppodealObjCBridge.AppodealSetAutocache(autoCache, nativeAdTypesForType(adTypes));
        }

        public void hide(int adTypes)
        {
            if ((nativeAdTypesForType(adTypes) & AppodealAdTypeBanner) > 0)
            {
                AppodealObjCBridge.AppodealHideBanner();
            }
        }

        public void hideBannerView()
        {
            AppodealObjCBridge.AppodealHideBannerView();
        }

        public void hideMrecView()
        {
            AppodealObjCBridge.AppodealHideMrecView();
        }

        public bool isPrecache(int adTypes)
        {
            return AppodealObjCBridge.AppodealIsPrecacheAd(nativeAdTypesForType(adTypes));
        }

        public void setSmartBanners(bool value)
        {
            AppodealObjCBridge.AppodealSetSmartBanners(value);
        }

        public bool isSmartBannersEnabled()
        {
            return AppodealObjCBridge.AppodealIsSmartBannersEnabled();
        }

        public void setBannerAnimation(bool value)
        {
            AppodealObjCBridge.AppodealSetBannerAnimation(value);
        }

        public void setTabletBanners(bool value)
        {
            AppodealObjCBridge.AppodealSetTabletBanners(value);
        }

        public void setBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            AppodealObjCBridge.AppodealSetBannerRotation(leftBannerRotation, rightBannerRotation);
        }

        public void setTesting(bool test)
        {
            AppodealObjCBridge.AppodealSetTestingEnabled(test);
        }

        public void setLogLevel(Appodeal.LogLevel level)
        {
            switch (level)
            {
                case Appodeal.LogLevel.None:
                {
                    AppodealObjCBridge.AppodealSetLogLevel(1);
                    break;
                }
                case Appodeal.LogLevel.Debug:
                {
                    AppodealObjCBridge.AppodealSetLogLevel(2);
                    break;
                }
                case Appodeal.LogLevel.Verbose:
                {
                    AppodealObjCBridge.AppodealSetLogLevel(3);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public void setChildDirectedTreatment(bool value)
        {
            AppodealObjCBridge.AppodealSetChildDirectedTreatment(value);
        }

        public void updateConsent(bool value)
        {
            AppodealObjCBridge.AppodealUpdateConsent(value);
        }

        public void updateConsent(Consent consent)
        {
            AppodealObjCBridge.AppodealUpdateConsentReport();
        }

        public void updateGdprConsent(Appodeal.GdprUserConsent consent)
        {
            switch (consent)
            {
                case Appodeal.GdprUserConsent.Unknown:
                {
                    AppodealObjCBridge.AppodealUpdateGdprConsent(0);
                    break;
                }
                case Appodeal.GdprUserConsent.Personalized:
                {
                    AppodealObjCBridge.AppodealUpdateGdprConsent(1);
                    break;
                }
                case Appodeal.GdprUserConsent.NonPersonalized:
                {
                    AppodealObjCBridge.AppodealUpdateGdprConsent(2);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(consent), consent, null);
            }
        }

        public void updateCcpaConsent(Appodeal.CcpaUserConsent consent)
        {
            switch (consent)
            {
                case Appodeal.CcpaUserConsent.Unknown:
                {
                    AppodealObjCBridge.AppodealUpdateCcpaConsent(0);
                    break;
                }
                case Appodeal.CcpaUserConsent.OptIn:
                {
                    AppodealObjCBridge.AppodealUpdateCcpaConsent(1);
                    break;
                }
                case Appodeal.CcpaUserConsent.OptOut:
                {
                    AppodealObjCBridge.AppodealUpdateCcpaConsent(2);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(consent), consent, null);
            }
        }

        public List<string> getNetworks(int adTypes)
        {
            var networksList = new List<string>();
            string [] splitNetworkStrings = AppodealObjCBridge.AppodealGetNetworks(nativeAdTypesForType(adTypes)).Split(',');
            splitNetworkStrings.ToList().ForEach(network => networksList.Add(network));
            return networksList;
        }

        public void disableNetwork(string network)
        {
            AppodealObjCBridge.AppodealDisableNetwork(network);
        }

        public void disableNetwork(string network, int adTypes)
        {
            AppodealObjCBridge.AppodealDisableNetworkForAdTypes(network, adTypes);
        }

        public void disableLocationPermissionCheck()
        {
            AppodealObjCBridge.AppodealDisableLocationPermissionCheck();
        }

        public string getVersion()
        {
            return AppodealObjCBridge.AppodealGetVersion();
        }

        public long getSegmentId()
        {
            return AppodealObjCBridge.AppodealGetSegmentId();
        }

        public bool canShow(int adTypes, string placement)
        {
            return AppodealObjCBridge.AppodealCanShowWithPlacement(nativeAdTypesForType(adTypes), placement);
        }

        public bool canShow(int adTypes)
        {
            return AppodealObjCBridge.AppodealCanShow(nativeAdTypesForType(adTypes));
        }

        public string getRewardCurrency(string placement)
        {
            return AppodealObjCBridge.AppodealGetRewardCurrency(placement);
        }

        public double getRewardAmount(string placement)
        {
            return AppodealObjCBridge.AppodealGetRewardAmount(placement);
        }

        public string getRewardCurrency()
        {
            return AppodealObjCBridge.AppodealGetRewardCurrency("default");
        }

        public double getRewardAmount()
        {
            return AppodealObjCBridge.AppodealGetRewardAmount("default");
        }

        public double getPredictedEcpm(int adType)
        {
            return AppodealObjCBridge.AppodealGetPredictedEcpm(nativeAdTypesForType(adType));
        }

        public void setCustomFilter(string name, bool value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterBool(name, value);
        }

        public void setCustomFilter(string name, int value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterInt(name, value);
        }

        public void setCustomFilter(string name, double value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterDouble(name, value);
        }

        public void setCustomFilter(string name, string value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterString(name, value);
        }

        public void resetCustomFilter(string name)
        {
            AppodealObjCBridge.AppodealResetCustomFilter(name);
        }

        public void setExtraData(string key, bool value)
        {
            AppodealObjCBridge.AppodealSetExtraDataBool(key, value);
        }

        public void setExtraData(string key, int value)
        {
            AppodealObjCBridge.AppodealSetExtraDataInt(key, value);
        }

        public void setExtraData(string key, double value)
        {
            AppodealObjCBridge.AppodealSetExtraDataDouble(key, value);
        }

        public void setExtraData(string key, string value)
        {
            AppodealObjCBridge.AppodealSetExtraDataString(key, value);
        }

        public void resetExtraData(string key)
        {
            AppodealObjCBridge.AppodealResetExtraData(key);
        }

        public void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            AppodealObjCBridge.AppodealSetTriggerPrecacheCallbacks(nativeAdTypesForType(adTypes), onLoadedTriggerBoth);
        }

        public bool isAutoCacheEnabled(int adType)
        {
            return AppodealObjCBridge.AppodealIsAutoCacheEnabled(nativeAdTypesForType(adType));
        }

        public void setUserId(string id)
        {
            AppodealObjCBridge.AppodealSetUserId(id);
        }

        public string getUserId()
        {
            return AppodealObjCBridge.AppodealGetUserId();
        }

        public void setUserAge(int age)
        {
            AppodealObjCBridge.AppodealSetUserAge(age);
        }

        public void setUserGender(UserSettings.Gender gender)
        {
            switch (gender)
            {
                case UserSettings.Gender.OTHER:
                {
                    AppodealObjCBridge.AppodealSetUserGender(0);
                    break;
                }
                case UserSettings.Gender.MALE:
                {
                    AppodealObjCBridge.AppodealSetUserGender(1);
                    break;
                }
                case UserSettings.Gender.FEMALE:
                {
                    AppodealObjCBridge.AppodealSetUserGender(2);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        public void trackInAppPurchase(double amount, string currency)
        {
            AppodealObjCBridge.AppodealTrackInAppPurchase(amount, currency);
        }

        public void logEvent(string eventName, Dictionary<string, object> eventParams)
        {
            var paramsFiltered = new Dictionary<string, object>();

            eventParams?.Keys.Where(key => eventParams[key] is int || eventParams[key] is double || eventParams[key] is string)
                .ToList().ForEach(key => paramsFiltered.Add(key, eventParams[key]));

            AppodealObjCBridge.AppodealLogEvent(eventName, dictionaryToString(paramsFiltered));
        }

        public void validateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            inAppPurchaseValidationListener = listener;
            AppodealObjCBridge.AppodealValidateInAppPurchase(purchase.getProductId(), purchase.getPrice(), purchase.getCurrency(), purchase.getTransactionId(), purchase.getAdditionalParameters(), (int) purchase.getPurchaseType(), inAppPurchaseValidationSucceeded, inAppPurchaseValidationFailed);
        }

        public void destroy(int adType)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void setSharedAdsInstanceAcrossActivities(bool value)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void setUseSafeArea(bool value)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void validatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void muteVideosIfCallsMuted(bool value)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void showTestScreen()
        {
            Debug.Log("Not Supported by iOS SDK");
        }
    }
}
#endif
