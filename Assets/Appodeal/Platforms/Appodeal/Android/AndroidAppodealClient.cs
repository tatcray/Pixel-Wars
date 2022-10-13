#if UNITY_ANDROID
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using ConsentManager;
using ConsentManager.Platforms.Android;

namespace AppodealAds.Unity.Android
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AndroidAppodealClient : IAppodealAdsClient
    {
        private AndroidJavaClass appodealClass;
        private AndroidJavaClass appodealUnityClass;
        private AndroidJavaObject appodealBannerInstance;
        private AndroidJavaObject activity;

        public const int NONE = 0;
        public const int INTERSTITIAL = 3;
        public const int BANNER = 4;
        public const int BANNER_BOTTOM = 8;
        public const int BANNER_TOP = 16;
        public const int BANNER_VIEW = 64;
        public const int BANNER_LEFT = 1024;
        public const int BANNER_RIGHT = 2048;
        public const int MREC = 256;
        public const int REWARDED_VIDEO = 128;

        private static int nativeAdTypesForType(int adTypes)
        {
            var nativeAdTypes = 0;

            if ((adTypes & Appodeal.INTERSTITIAL) > 0)
            {
                nativeAdTypes |= Appodeal.INTERSTITIAL;
            }

            if ((adTypes & Appodeal.BANNER) > 0)
            {
                nativeAdTypes |= Appodeal.BANNER;
            }

            if ((adTypes & Appodeal.BANNER_VIEW) > 0)
            {
                nativeAdTypes |= Appodeal.BANNER_VIEW;
            }

            if ((adTypes & Appodeal.BANNER_TOP) > 0)
            {
                nativeAdTypes |= Appodeal.BANNER_TOP;
            }
            
            if ((adTypes & Appodeal.BANNER_LEFT) > 0)
            {
                nativeAdTypes |= Appodeal.BANNER_LEFT;
            }

            if ((adTypes & Appodeal.BANNER_RIGHT) > 0)
            {
                nativeAdTypes |= Appodeal.BANNER_RIGHT;
            }

            if ((adTypes & Appodeal.BANNER_BOTTOM) > 0)
            {
                nativeAdTypes |= Appodeal.BANNER_BOTTOM;
            }

            if ((adTypes & Appodeal.MREC) > 0)
            {
                nativeAdTypes |= 256;
            }

            if ((adTypes & Appodeal.REWARDED_VIDEO) > 0)
            {
                nativeAdTypes |= Appodeal.REWARDED_VIDEO;
            }

            return nativeAdTypes;
        }

        private AndroidJavaClass getAppodealClass()
        {
            return appodealClass ?? (appodealClass = new AndroidJavaClass("com.appodeal.ads.Appodeal"));
        }

        public AndroidJavaClass getAppodealUnityClass()
        {
            return appodealUnityClass ?? (appodealUnityClass = new AndroidJavaClass("com.appodeal.unity.AppodealUnity"));
        }

        private AndroidJavaObject getAppodealBannerInstance()
        {
            return appodealBannerInstance ?? (appodealBannerInstance = new AndroidJavaClass("com.appodeal.ads.AppodealUnityBannerView").CallStatic<AndroidJavaObject>("getInstance"));
        }

        private AndroidJavaObject getActivity()
        {
            return activity ?? (activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
        }

        public void initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            getAppodealClass().CallStatic("setFramework", "unity", Appodeal.getPluginVersion(), Appodeal.getUnityVersion());
            getAppodealClass().CallStatic("initialize", getActivity(), appKey, nativeAdTypesForType(adTypes), new AppodealInitializationCallback(listener));
        }

        public void initialize(string appKey, int adTypes)
        {
            initialize(appKey, adTypes, true);
        }

        public void initialize(string appKey, int adTypes, bool hasConsent)
        {
            getAppodealClass().CallStatic("setFramework", "unity", Appodeal.getPluginVersion(), Appodeal.getUnityVersion());
            getAppodealClass().CallStatic("initialize", getActivity(), appKey, nativeAdTypesForType(adTypes), hasConsent);
        }

        public void initialize(string appKey, int adTypes, Consent consent)
        {
            getAppodealClass().CallStatic("setFramework", "unity", Appodeal.getPluginVersion(), Appodeal.getUnityVersion());
            var androidConsent = (AndroidConsent) consent.getConsent();
            getAppodealClass().CallStatic("initialize", getActivity(), appKey, nativeAdTypesForType(adTypes), androidConsent.getConsent());
        }

        public bool isInitialized(int adType)
        {
            return getAppodealClass().CallStatic<bool>("isInitialized", nativeAdTypesForType(adType));
        }

        public bool show(int adTypes)
        {
            return getAppodealClass().CallStatic<bool>("show", getActivity(), nativeAdTypesForType(adTypes));
        }

        public bool show(int adTypes, string placement)
        {
            return getAppodealClass().CallStatic<bool>("show", getActivity(), nativeAdTypesForType(adTypes), placement);
        }

        public bool showBannerView(int YAxis, int XAxis, string Placement)
        {
            return getAppodealBannerInstance().Call<bool>("showBannerView", getActivity(), XAxis, YAxis, Placement);
        }

        public bool showMrecView(int YAxis, int XAxis, string Placement)
        {
            return getAppodealBannerInstance().Call<bool>("showMrecView", getActivity(), XAxis, YAxis, Placement);
        }

        public bool isLoaded(int adTypes)
        {
            return getAppodealClass().CallStatic<bool>("isLoaded", nativeAdTypesForType(adTypes));
        }

        public void cache(int adTypes)
        {
            getAppodealClass().CallStatic("cache", getActivity(), nativeAdTypesForType(adTypes));
        }

        public void hide(int adTypes)
        {
            getAppodealClass().CallStatic("hide", getActivity(), nativeAdTypesForType(adTypes));
        }

        public void hideBannerView()
        {
            getAppodealBannerInstance().Call("hideBannerView", getActivity());
        }

        public void hideMrecView()
        {
            getAppodealBannerInstance().Call("hideMrecView", getActivity());
        }

        public bool isPrecache(int adTypes)
        {
            return getAppodealClass().CallStatic<bool>("isPrecache", nativeAdTypesForType(adTypes));
        }

        public void setAutoCache(int adTypes, bool autoCache)
        {
            getAppodealClass().CallStatic("setAutoCache", nativeAdTypesForType(adTypes), autoCache);
        }

        public void setSmartBanners(bool value)
        {
            getAppodealClass().CallStatic("setSmartBanners", value);
        }

        public bool isSmartBannersEnabled()
        {
            return getAppodealClass().CallStatic<bool>("isSmartBannersEnabled");
        }

        public void setBannerAnimation(bool value)
        {
            getAppodealClass().CallStatic("setBannerAnimation", value);
        }

        public void setTabletBanners(bool value)
        {
            getAppodealClass().CallStatic("set728x90Banners", value);
        }
        
        public void setBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            getAppodealClass().CallStatic("setBannerRotation", leftBannerRotation, rightBannerRotation);
        }

        public void setTesting(bool test)
        {
            getAppodealClass().CallStatic("setTesting", test);
        }

        public void setLogLevel(Appodeal.LogLevel logging)
        {
            var logLevel = new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel");
            switch (logging)
            {
                case Appodeal.LogLevel.None:
                {
                    getAppodealClass().CallStatic("setLogLevel",
                        logLevel.CallStatic<AndroidJavaObject>("fromInteger", Helper.getJavaObject(0)));
                    break;
                }
                case Appodeal.LogLevel.Debug:
                {
                    getAppodealClass().CallStatic("setLogLevel",
                        logLevel.CallStatic<AndroidJavaObject>("fromInteger", Helper.getJavaObject(1)));
                    break;
                }
                case Appodeal.LogLevel.Verbose:
                {
                    getAppodealClass().CallStatic("setLogLevel",
                        logLevel.CallStatic<AndroidJavaObject>("fromInteger", Helper.getJavaObject(2)));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(logging), logging, null);
            }
        }

        public void setChildDirectedTreatment(bool value)
        {
            getAppodealClass().CallStatic("setChildDirectedTreatment", Helper.getJavaObject(value));
        }

        public void updateConsent(bool value)
        {
            getAppodealClass().CallStatic("updateConsent", Helper.getJavaObject(value));
        }

        public void updateConsent(Consent consent)
        {
            var androidConsent = (AndroidConsent) consent.getConsent();
            getAppodealClass().CallStatic("updateConsent", androidConsent.getConsent());
        }

        public void updateGdprConsent(Appodeal.GdprUserConsent consent)
        {
            var gdprConsent = new AndroidJavaClass("com.appodeal.ads.regulator.GDPRUserConsent");
            switch (consent)
            {
                case Appodeal.GdprUserConsent.Unknown:
                {
                    getAppodealClass().CallStatic("updateGDPRUserConsent", gdprConsent.CallStatic<AndroidJavaObject>("valueOf", "Unknown"));
                    break;
                }
                case Appodeal.GdprUserConsent.Personalized:
                {
                    getAppodealClass().CallStatic("updateGDPRUserConsent", gdprConsent.CallStatic<AndroidJavaObject>("valueOf", "Personalized"));
                    break;
                }
                case Appodeal.GdprUserConsent.NonPersonalized:
                {
                    getAppodealClass().CallStatic("updateGDPRUserConsent", gdprConsent.CallStatic<AndroidJavaObject>("valueOf", "NonPersonalized"));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(consent), consent, null);
            }
        }

        public void updateCcpaConsent(Appodeal.CcpaUserConsent consent)
        {
            var ccpaConsent = new AndroidJavaClass("com.appodeal.ads.regulator.CCPAUserConsent");
            switch (consent)
            {
                case Appodeal.CcpaUserConsent.Unknown:
                {
                    getAppodealClass().CallStatic("updateCCPAUserConsent", ccpaConsent.CallStatic<AndroidJavaObject>("valueOf", "Unknown"));
                    break;
                }
                case Appodeal.CcpaUserConsent.OptIn:
                {
                    getAppodealClass().CallStatic("updateCCPAUserConsent", ccpaConsent.CallStatic<AndroidJavaObject>("valueOf", "OptIn"));
                    break;
                }
                case Appodeal.CcpaUserConsent.OptOut:
                {
                    getAppodealClass().CallStatic("updateCCPAUserConsent", ccpaConsent.CallStatic<AndroidJavaObject>("valueOf", "OptOut"));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(consent), consent, null);
            }
        }

        public void disableNetwork(string network)
        {
            getAppodealClass().CallStatic("disableNetwork", network);
        }

        public void disableNetwork(string network, int adTypes)
        {
            getAppodealClass().CallStatic("disableNetwork", network, nativeAdTypesForType(adTypes));
        }

        public void disableLocationPermissionCheck()
        {
            Debug.Log("Not supported on Android platform");
        }

        public void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            getAppodealClass().CallStatic("setTriggerOnLoadedOnPrecache", nativeAdTypesForType(adTypes),
                onLoadedTriggerBoth);
        }

        public void muteVideosIfCallsMuted(bool value)
        {
            getAppodealClass().CallStatic("muteVideosIfCallsMuted", value);
        }

        public void showTestScreen()
        {
            getAppodealClass().CallStatic("startTestActivity", getActivity());
        }

        public string getVersion()
        {
            return getAppodealClass().CallStatic<string>("getVersion");
        }

        public long getSegmentId()
        {
            return getAppodealClass().CallStatic<long>("getSegmentId");
        }

        public bool canShow(int adTypes)
        {
            return getAppodealClass().CallStatic<bool>("canShow", nativeAdTypesForType(adTypes));
        }

        public bool canShow(int adTypes, string placement)
        {
            return getAppodealClass().CallStatic<bool>("canShow", nativeAdTypesForType(adTypes), placement);
        }
        
        public void setCustomFilter(string name, bool value)
        {
            getAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void setCustomFilter(string name, int value)
        {
            getAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void setCustomFilter(string name, double value)
        {
            getAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void setCustomFilter(string name, string value)
        {
            getAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void resetCustomFilter(string name)
        {
            getAppodealClass().CallStatic("setCustomFilter", name, null);
        }

        public void setExtraData(string key, bool value)
        {
            getAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void setExtraData(string key, int value)
        {
            getAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void setExtraData(string key, double value)
        {
            getAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void setExtraData(string key, string value)
        {
            getAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void resetExtraData(string key)
        {
            getAppodealClass().CallStatic("setExtraData", key, null);
        }

        public void trackInAppPurchase(double amount, string currency)
        {
            getAppodealClass().CallStatic("trackInAppPurchase", getActivity(), amount, currency);
        }

        public List<string> getNetworks(int adTypes)
        {
            var networks = getAppodealClass().CallStatic<AndroidJavaObject>("getNetworks", getActivity(), nativeAdTypesForType(adTypes));
            int countOfNetworks = networks.Call<int>("size");
            var networksList = new List<string>();
            for(int i = 0; i < countOfNetworks; i++)
            {
                networksList.Add(networks.Call<string>("get", i));
            }
            return networksList;
        }

        public string getRewardCurrency(string placement)
        {
            var reward =
                getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", placement);
            return reward.Get<string>("second");
        }

        public double getRewardAmount(string placement)
        {
            var reward =
                getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters", placement);
            var doubleValue = reward.Get<AndroidJavaObject>("first");
            return doubleValue.Call<double>("doubleValue");
        }

        public string getRewardCurrency()
        {
            var reward = getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters");
            return reward.Get<string>("second");
        }

        public double getRewardAmount()
        {
            var reward = getAppodealClass().CallStatic<AndroidJavaObject>("getRewardParameters");
            var doubleValue = reward.Get<AndroidJavaObject>("first");
            return doubleValue.Call<double>("doubleValue");
        }

        public double getPredictedEcpm(int adType)
        {
            return getAppodealClass().CallStatic<double>("getPredictedEcpm", adType);
        }

        public void destroy(int adTypes)
        {
            getAppodealClass().CallStatic("destroy", nativeAdTypesForType(adTypes));
        }

        public void setUserId(string id)
        {
            getAppodealClass().CallStatic("setUserId", id);
        }

        public string getUserId()
        {
            return getAppodealClass().CallStatic<string>("getUserId");
        }

        public void setUserAge(int age)
        {
            getAppodealClass().CallStatic("setUserAge", age);
        }

        public void setUserGender(UserSettings.Gender gender)
        {
            switch (gender)
            {
                case UserSettings.Gender.OTHER:
                {
                    getAppodealClass().CallStatic("setUserGender",
                        new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>(
                            "OTHER"));
                    break;
                }
                case UserSettings.Gender.MALE:
                {
                    getAppodealClass().CallStatic("setUserGender",
                        new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>(
                            "MALE"));
                    break;
                }
                case UserSettings.Gender.FEMALE:
                {
                    getAppodealClass().CallStatic("setUserGender",
                        new AndroidJavaClass("com.appodeal.ads.UserSettings$Gender").GetStatic<AndroidJavaObject>(
                            "FEMALE"));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        public void setInterstitialCallbacks(IInterstitialAdListener listener)
        {
            getAppodealClass().CallStatic("setInterstitialCallbacks", new AppodealInterstitialCallbacks(listener));
        }

        public void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            getAppodealClass().CallStatic("setRewardedVideoCallbacks", new AppodealRewardedVideoCallbacks(listener));
        }

        public void setBannerCallbacks(IBannerAdListener listener)
        {
            getAppodealClass().CallStatic("setBannerCallbacks", new AppodealBannerCallbacks(listener));
        }

        public void setMrecCallbacks(IMrecAdListener listener)
        {
            getAppodealClass().CallStatic("setMrecCallbacks", new AppodealMrecCallbacks(listener));
        }
        
        public void setSharedAdsInstanceAcrossActivities(bool value)
        {
            getAppodealClass().CallStatic("setSharedAdsInstanceAcrossActivities", value);
        }

        public void setUseSafeArea(bool value)
        {
            getAppodealClass().CallStatic("setUseSafeArea", value);
        }

        public bool isAutoCacheEnabled(int adType)
        {
            return  getAppodealClass().CallStatic<bool>("isAutoCacheEnabled", nativeAdTypesForType(adType));
        }

        public void logEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (eventParams == null)
            {
                getAppodealClass().CallStatic("logEvent", eventName, null);
            }
            else
            {
                var paramsFiltered = new Dictionary<string, object>();

                eventParams.Keys.Where(key => eventParams[key] is int || eventParams[key] is double || eventParams[key] is string)
                    .ToList().ForEach(key => paramsFiltered.Add(key, eventParams[key]));

                var map = new AndroidJavaObject("java.util.HashMap");
                
                foreach (var entry in paramsFiltered)
                {
                    map.Call<AndroidJavaObject>("put", entry.Key, Helper.getJavaObject(entry.Value));
                }

                getAppodealClass().CallStatic("logEvent", eventName, map);
            }
        }

        public void validatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            var androidPurchase = purchase.NativeInAppPurchase as AndroidPlayStoreInAppPurchase;
            if (androidPurchase == null) return;
            getAppodealClass().CallStatic("validateInAppPurchase", getActivity(), androidPurchase.getInAppPurchase(), new InAppPurchaseValidationCallbacks(listener));
        }

        public void validateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            Debug.Log("Not supported on Android platform");
        }
    }

    public static class Helper
    {
        public static object getJavaObject(object value)
        {
            if (value is string)
            {
                return value;
            }

            if (value is char)
            {
                return new AndroidJavaObject("java.lang.Character", value);
            }

            if (value is bool)
            {
                return new AndroidJavaObject("java.lang.Boolean", value);
            }

            if (value is int)
            {
                return new AndroidJavaObject("java.lang.Integer", value);
            }

            if (value is long)
            {
                return new AndroidJavaObject("java.lang.Long", value);
            }

            if (value is float)
            {
                return new AndroidJavaObject("java.lang.Float", value);
            }

            if (value is double)
            {
                return new AndroidJavaObject("java.lang.Float", value);
            }

            return null;
        }
    }
}
#endif
