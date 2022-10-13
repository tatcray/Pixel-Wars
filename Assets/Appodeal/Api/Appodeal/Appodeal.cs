using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System;
using AppodealAds.Unity.Common;
using ConsentManager;
using UnityEngine;

namespace AppodealAds.Unity.Api
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class AppodealNetworks
    {
        public const string VUNGLE = "vungle";
        public const string SMAATO = "smaato";
        public const string INMOBI = "inmobi";
        public const string FYBER = "fyber";
        public const string STARTAPP = "startapp";
        public const string TAPJOY = "tapjoy";
        public const string APPLOVIN = "applovin";
        public const string ADCOLONY = "adcolony";
        public const string MY_TARGET = "my_target";
        public const string BIDMACHINE = "bidmachine";
        public const string FLURRY = "flurry";
        public const string AMAZON_ADS = "amazon_ads";
        public const string ADMOB = "admob";
        public const string UNITY_ADS = "unity_ads";
        public const string FACEBOOK = "facebook";
        public const string YANDEX = "yandex";
        public const string APPODEAL = "appodeal";
        public const string IRONSOURCE = "ironsource";
        public const string A4G = "a4g";
        public const string MOPUB = "mopub";
        public const string CHARTBOOST = "chartboost";
        public const string MRAID = "mraid";
        public const string MINTEGRAL = "mintegral";
        public const string NAST = "nast";
        public const string OGURY = "ogury";
        public const string VAST = "vast";
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public static class Appodeal
    {
        #region AdTypes

        public const int NONE = 0;
        public const int INTERSTITIAL = 3;
        public const int BANNER = 4;
        public const int BANNER_BOTTOM = 8;
        public const int BANNER_TOP = 16;
        public const int BANNER_VIEW = 64;
        public const int MREC = 512;
        public const int REWARDED_VIDEO = 128;
        public const int BANNER_LEFT = 1024;
        public const int BANNER_RIGHT = 2048;

        public const int BANNER_HORIZONTAL_SMART = -1;
        public const int BANNER_HORIZONTAL_CENTER = -2;
        public const int BANNER_HORIZONTAL_RIGHT = -3;
        public const int BANNER_HORIZONTAL_LEFT = -4;

        #endregion

        /// <summary>
        /// The version for the Appodeal Unity SDK, which includes specific versions of the Appodeal Android and iOS SDKs.
        /// </summary>
        public const string APPODEAL_PLUGIN_VERSION = "3.0.0";
        
        public enum LogLevel
        {
            None,
            Debug,
            Verbose
        }

        public enum GdprUserConsent
        {
            Unknown,
            Personalized,
            NonPersonalized
        }

        public enum CcpaUserConsent
        {
            Unknown,
            OptIn,
            OptOut
        }

        public enum PlayStorePurchaseType
        {
            Subs,
            InApp
        }

        public enum AppStorePurchaseType
        {
            Consumable,
            NonConsumable,
            AutoRenewableSubscription,
            NonRenewingSubscription
        }

        private static IAppodealAdsClient client;

        private static IAppodealAdsClient getInstance()
        {
            return client ?? (client = AppodealAdsClientFactory.GetAppodealAdsClient());
        }

        /// <summary>
        /// <para>
        /// Initializes the relevant (Android or iOS) Appodeal SDK.
        /// See <see langword="OnAppodealInitialized()"/> for resulting triggered event.
        /// </para>
        /// <example>To initialize only interstitials use:<code>Appodeal.initialize(appKey, Appodeal.INTERSTITIAL, this);</code></example>
        /// <example>To initialize only banners use:<code>Appodeal.initialize(appKey, Appodeal.BANNER, this);</code></example>
        /// <example>To initialize only rewarded video use:<code>Appodeal.initialize(appKey, Appodeal.REWARDED_VIDEO, this);</code></example>
        /// <example>To initialize only 300*250 banners use:<code>Appodeal.initialize(appKey, Appodeal.MREC, this);</code></example>
        /// <example>To initialize multiple ad types use the <see langword="|"/> operator:<code>Appodeal.Initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER, this);</code></example>
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started#UnitySDK.GetStarted-Step3Step3.InitializeSDK"/> for more details.</remarks>
        /// <param name="appKey">appodeal app key that was assigned to your app when it was created.</param>
        /// <param name="adTypes">type of advertisement you want to initialize.</param>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IAppodealInitializeListener interface.</param>
        public static void initialize(string appKey, int adTypes, IAppodealInitializationListener listener = null)
        {
            getInstance().initialize(appKey, adTypes, listener);
        }

        /// <summary>
        /// Check if ad type is initialized
        /// See <see cref="Appodeal.isInitialized"/> for resulting triggered event.
        /// <param name="adType">adType type of advertising.</param>
        /// </summary>
        public static bool isInitialized(int adType)
        {
            return getInstance().isInitialized(adType);
        }

        /// <summary>
        /// Update consent value for ad networks in Appodeal SDK
        /// See <see cref="Appodeal.updateConsent"/> for resulting triggered event.
        /// <param name="consent"> Consent user has given consent to the processing of personal data relating to him or her. https://www.eugdpr.org/.</param>
        /// </summary>
        public static void updateConsent(Consent consent)
        {
            getInstance().updateConsent(consent);
        }

        /// <summary>
        /// <para>Updates consent value (GDPR regulation) used by ad networks and services of Appodeal SDK.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Calling this method before SDK initialization will result in disabling Consent Manager window showing. However, Consent Manager still will be synchronized using the consent object passed in this method.</remarks>
        /// <param name="consent">user's consent on processing of their personal data. https://www.eugdpr.org</param>
        public static void updateGdprConsent(GdprUserConsent consent)
        {
            getInstance().updateGdprConsent(consent);
        }

        /// <summary>
        /// <para>Updates consent value (CCPA regulation) used by ad networks and services of Appodeal SDK.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Calling this method before SDK initialization will result in disabling Consent Manager window showing. However, Consent Manager still will be synchronized using the consent object passed in this method.</remarks>
        /// <param name="consent">user's consent on processing of their personal data. https://oag.ca.gov/privacy/ccpa</param>
        public static void updateCcpaConsent(CcpaUserConsent consent)
        {
            getInstance().updateCcpaConsent(consent);
        }

        /// <summary>
        /// Check if auto cache enabled for  this ad type
        /// See <see cref="Appodeal.isAutoCacheEnabled"/> for resulting triggered event.
        /// <param name="adType">adType type of advertising.</param>
        /// </summary>
        public static bool isAutoCacheEnabled(int adType)
        {
            return getInstance().isAutoCacheEnabled(adType);
        }
        
        /// <summary>
        /// Set Interstitial ads callbacks
        /// See <see cref="Appodeal.setInterstitialCallbacks"/> for resulting triggered event.
        /// <param name="listener">callbacks implementation of Appodeal/Common/Appodeal/IInterstitialAdListener.</param>
        /// </summary>
        public static void setInterstitialCallbacks(IInterstitialAdListener listener)
        {
            getInstance().setInterstitialCallbacks(listener);
        }

        /// <summary>
        /// Set Interstitial ads callbacks
        /// See <see cref="Appodeal.setRewardedVideoCallbacks"/> for resulting triggered event.
        /// <param name="listener">callbacks implementation of Appodeal/Common/Appodeal/IRewardedVideoAdListener.</param>
        /// </summary>
        public static void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            getInstance().setRewardedVideoCallbacks(listener);
        }

        /// <summary>
        /// Set Interstitial ads callbacks
        /// See <see cref="Appodeal.setBannerCallbacks"/> for resulting triggered event.
        /// <param name="listener">callbacks implementation of Appodeal/Common/Appodeal/IBannerAdListener.</param>
        /// </summary>
        public static void setBannerCallbacks(IBannerAdListener listener)
        {
            getInstance().setBannerCallbacks(listener);
        }

        /// <summary>
        /// Set Interstitial ads callbacks
        /// See <see cref="Appodeal.setMrecCallbacks"/> for resulting triggered event.
        /// <param name="listener">callbacks implementation of Appodeal/Common/Appodeal/IMrecAdListener.</param>
        /// </summary>
        public static void setMrecCallbacks(IMrecAdListener listener)
        {
            getInstance().setMrecCallbacks(listener);
        }
        
        /// <summary>
        /// Start caching ads.
        /// See <see cref="Appodeal.cache"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising.</param>
        /// </summary>
        public static void cache(int adTypes)
        {
            getInstance().cache(adTypes);
        }

        /// <summary>
        /// Show advertising.
        /// See <see cref="Appodeal.show"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising.</param>
        /// </summary>
        public static bool show(int adTypes)
        {
            return getInstance().show(adTypes);
        }

        /// <summary>
        /// Show advertising.
        /// See <see cref="Appodeal.show"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising.</param>
        /// <param name="placement">type of advertising you want to show.</param>
        /// </summary>
        public static bool show(int adTypes, string placement)
        {
            return getInstance().show(adTypes, placement);
        }

        /// <summary>
        /// Show banner view.
        /// See <see cref="Appodeal.showBannerView"/> for resulting triggered event.
        /// <param name="YAxis">y position for banner view.</param>
        /// <param name="XGravity">x position for banner view.</param>
        /// <param name="placement">type of advertising you want to show.</param>
        /// </summary>
        public static bool showBannerView(int YAxis, int XGravity, string placement)
        {
            return getInstance().showBannerView(YAxis, XGravity, placement);
        }
        
        /// <summary>
        /// Show mrec view.
        /// See <see cref="Appodeal.showMrecView"/> for resulting triggered event.
        /// <param name="YAxis">y position for mrec view.</param>
        /// <param name="XGravity">x position for mrec view.</param>
        /// <param name="placement">type of advertising you want to show.</param>
        /// </summary>
        public static bool showMrecView(int YAxis, int XGravity, string placement)
        {
            return getInstance().showMrecView(YAxis, XGravity, placement);
        }
        
        /// <summary>
        /// Hide advertising.
        /// See <see cref="Appodeal.hide"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising  Appodeal.BANNER</param>
        /// </summary>
        public static void hide(int adTypes)
        {
            getInstance().hide(adTypes);
        }

        /// <summary>
        /// Hide Banner View.
        /// See <see cref="Appodeal.hideBannerView"/> for resulting triggered event.
        /// </summary>
        public static void hideBannerView()
        {
            getInstance().hideBannerView();
        }

        /// <summary>
        /// Hide Mrec view.
        /// See <see cref="Appodeal.hideMrecView"/> for resulting triggered event.
        /// </summary>
        public static void hideMrecView()
        {
            getInstance().hideMrecView();
        }
        
        /// <summary>
        /// Start or stop auto caching new ads when current ads was shown..
        /// See <see cref="Appodeal.setAutoCache"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising </param>
        /// <param name="autoCache">true to use auto cache, false to not.</param>
        /// </summary>
        public static void setAutoCache(int adTypes, bool autoCache)
        {
            getInstance().setAutoCache(adTypes, autoCache);
        }
        
        /// <summary>
        /// Triggering onLoaded callback when precache loaded.
        /// See <see cref="Appodeal.setTriggerOnLoadedOnPrecache"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising </param>
        /// <param name="onLoadedTriggerBoth">true - onLoaded will trigger when precache or normal ad were loaded.
        ///                         false - onLoaded will trigger only when normal ad was loaded (default).</param>
        /// </summary>
        public static void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            getInstance().setTriggerOnLoadedOnPrecache(adTypes, onLoadedTriggerBoth);
        }

        /// <summary>
        /// Checking if ad is loaded. Return true if ads currently loaded and can be shown.
        /// See <see cref="Appodeal.isLoaded"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising </param>
        /// </summary>
        public static bool isLoaded(int adTypes)
        {
            return getInstance().isLoaded(adTypes);
        }

        /// <summary>
        /// Checking if loaded ad is precache. Return true if currently loaded ads is precache.
        /// See <see cref="Appodeal.isPrecache"/> for resulting triggered event.
        /// <param name="adTypes">adType type of advertising. Currently supported only for interstitials. </param>
        /// </summary>
        public static bool isPrecache(int adTypes)
        {
            return getInstance().isPrecache(adTypes);
        }

        /// <summary>
        /// Enabling or disabling smart banners (Enabled by default).
        /// See <see cref="Appodeal.setSmartBanners"/> for resulting triggered event.
        /// <param name="enabled">enabled enabling or disabling loading smart banners.</param>
        /// </summary>
        public static void setSmartBanners(bool enabled)
        {
            getInstance().setSmartBanners(enabled);
        }

        /// <summary>
        /// <para>
        /// Checks whether or not smart banners feature is enabled. (It is <see langword="true"/> by default).
        /// </para>
        /// It is usually used along with the <see cref="setSmartBanners"/> method.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2684869-banner-sizes"/> for more details.</remarks>
        /// <returns>True if smart banners are enabled, otherwise - false.</returns>
        public static bool isSmartBannersEnabled()
        {
            return getInstance().isSmartBannersEnabled();
        }

        /// <summary>
        /// Enabling or disabling 728*90 banners (Disabled by default).
        /// See <see cref="Appodeal.setTabletBanners"/> for resulting triggered event.
        /// <param name="enabled">enabled enabling or disabling loading 728*90 banners.</param>
        /// </summary>
        public static void setTabletBanners(bool enabled)
        {
            getInstance().setTabletBanners(enabled);
        }

        /// <summary>
        /// Enabling animation of banners (Enabled by default).
        /// See <see cref="Appodeal.setBannerAnimation"/> for resulting triggered event.
        /// <param name="enabled">animate enabling or disabling animations.</param>
        /// </summary>
        public static void setBannerAnimation(bool enabled)
        {
            getInstance().setBannerAnimation(enabled);
        }

        /// <summary>
        /// Setting banners inverse rotation (by default: left = -90, right = 90).
        /// See <see cref="Appodeal.setBannerRotation"/> for resulting triggered event.
        /// <param name="leftBannerRotation">leftBannerRotation rotation for Appodeal.BANNER_LEFT.</param>
        /// <param name="rightBannerRotation">leftBannerRotation rotation for Appodeal.BANNER_RIGHT.</param>
        /// </summary>
        public static void setBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            getInstance().setBannerRotation(leftBannerRotation, rightBannerRotation);
        }
        
        /// <summary>
        /// Tracks in-app purchase information and sends info to our servers for analytics.
        /// See <see cref="Appodeal.trackInAppPurchase"/> for resulting triggered event.
        /// <param name="amount">amount of purchase.</param>
        /// <param name="currency">currency of purchase.</param>
        /// </summary>
        public static void trackInAppPurchase(double amount, string currency)
        {
            getInstance().trackInAppPurchase(amount, currency);
        }

        /// <summary>
        /// <para>
        /// Gets a list of available ad networks for certain ad type.
        /// </para>
        /// <example>Usage example:<code>Appodeal.getNetworks(Appodeal.INTERSTITIAL);</code></example>
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>List of available ad networks for the specified ad type.</returns>
        public static List<string> getNetworks(int adType)
        {
            return getInstance().getNetworks(adType);
        }

        /// <summary>
        /// Disabling specified network for all ad types.
        /// See <see cref="Appodeal.disableNetwork"/> for resulting triggered event.
        /// <param name="network">network name.</param>
        /// </summary>
        public static void disableNetwork(string network)
        {
            getInstance().disableNetwork(network);
        }
        
        /// <summary>
        /// Disabling specified network for specified ad types.
        /// See <see cref="Appodeal.disableNetwork"/> for resulting triggered event.
        /// <param name="network">network name.</param>
        /// </summary>
        public static void disableNetwork(string network, int adType)
        {
            getInstance().disableNetwork(network, adType);
        }
        
        /// <summary>
        /// Disabling location tracking (for iOS platform only).
        /// See <see cref="Appodeal.disableLocationPermissionCheck"/> for resulting triggered event.
        /// </summary>
        public static void disableLocationPermissionCheck()
        {
            getInstance().disableLocationPermissionCheck();
        }

        /// <summary>
        /// Set user id.
        /// See <see cref="Appodeal.setUserId"/> for resulting triggered event.
        /// <param name="id">user id.</param>
        /// </summary>
        public static void setUserId(string id)
        {
            getInstance().setUserId(id);
        }

        /// <summary>Gets user id.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data"/> for more details.</remarks>
        /// <returns>User id as string.</returns>
        public static string getUserId()
        {
            return getInstance().getUserId();
        }

        /// <summary>Gets active segment id.</summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/collections/107529-segments"/> for more details.</remarks>
        /// <returns>Segment id as long.</returns>
        public static long getSegmentId()
        {
            return getInstance().getSegmentId();
        }
        
        /// <summary>
        /// Set test mode.
        /// See <see cref="Appodeal.setTesting"/> for resulting triggered event.
        /// </summary>
        public static void setTesting(bool test)
        {
            getInstance().setTesting(test);
        }

        /// <summary>
        /// Set log level. All logs will be written with tag "Appodeal".
        /// See <see cref="Appodeal.setLogLevel"/> for resulting triggered event.
        /// <param name="log">logLevel log level .</param>
        /// </summary>
        public static void setLogLevel(LogLevel log)
        {
            getInstance().setLogLevel(log);
        }
        
        /// <summary>
        /// Set custom segment filter.
        /// See <see cref="Appodeal.setCustomFilter"/> for resulting triggered event.
        /// <param name="name">name  name of the filter.</param>
        /// <param name="value">value filter value.</param>
        /// </summary>
        public static void setCustomFilter(string name, bool value)
        {
            getInstance().setCustomFilter(name, value);
        }
        
        /// <summary>
        /// Set custom segment filter.
        /// See <see cref="Appodeal.setCustomFilter"/> for resulting triggered event.
        /// <param name="name">name  name of the filter.</param>
        /// <param name="value">value filter value.</param>
        /// </summary>
        public static void setCustomFilter(string name, int value)
        {
            getInstance().setCustomFilter(name, value);
        }

        /// <summary>
        /// Set custom segment filter.
        /// See <see cref="Appodeal.setCustomFilter"/> for resulting triggered event.
        /// <param name="name">name  name of the filter.</param>
        /// <param name="value">value filter value.</param>
        /// </summary>
        public static void setCustomFilter(string name, double value)
        {
            getInstance().setCustomFilter(name, value);
        }

        /// <summary>
        /// Set custom segment filter.
        /// See <see cref="Appodeal.setCustomFilter"/> for resulting triggered event.
        /// <param name="name">name  name of the filter.</param>
        /// <param name="value">value filter value.</param>
        /// </summary>
        public static void setCustomFilter(string name, string value)
        {
            getInstance().setCustomFilter(name, value);
        }

        /// <summary>
        /// <para>
        /// Resets custom filter value by the provided key.
        /// </para>
        /// See <see href="https://faq.appodeal.com/en/articles/1133533-segment-filters"/> for more details.
        /// </summary>
        /// <remarks>Use it to remove a filter, that was previously set via one of the <see langword="setCustomFilter()"/> methods.</remarks>
        /// <param name="name">name of the filter.</param>
        public static void resetCustomFilter(string name)
        {
            getInstance().resetCustomFilter(name);
        }
        
        /// <summary>
        /// Check if ad with specific ad type can be shown with placement.
        /// See <see cref="Appodeal.canShow"/> for resulting triggered event.
        /// <param name="adTypes">type of advertising.</param>
        /// </summary> 
        public static bool canShow(int adTypes)
        {
            return getInstance().canShow(adTypes);
        }

        /// <summary>
        /// Check if ad with specific ad type can be shown with placement.
        /// See <see cref="Appodeal.canShow"/> for resulting triggered event.
        /// <param name="adTypes">type of advertising.</param>
        /// <param name="placement">placement name.</param>
        /// </summary> 
        public static bool canShow(int adTypes, string placement)
        {
            return getInstance().canShow(adTypes, placement);
        }
        
        /// <summary>
        /// Get reward parameters.
        /// See <see cref="Appodeal.getRewardParameters"/> for resulting triggered event.
        /// </summary> 
        public static KeyValuePair<string, double> getRewardParameters()
        {
            return new KeyValuePair<string, double>(getInstance().getRewardCurrency(), getInstance().getRewardAmount());
        }

        /// <summary>
        /// Get reward parameters for placement.
        /// See <see cref="Appodeal.getRewardParameters"/> for resulting triggered event.
        /// <param name="placement">placement name.</param>
        /// </summary> 
        public static KeyValuePair<string, double> getRewardParameters(string placement)
        {
            return new KeyValuePair<string, double>(getInstance().getRewardCurrency(placement),
                getInstance().getRewardAmount(placement));
        }
        
        /// <summary>
        /// Mute video if calls muted on device (supports only for Android platform).
        /// See <see cref="Appodeal.muteVideosIfCallsMuted"/> for resulting triggered event.
        /// <param name="value">true - mute videos if call volume is 0.</param>
        /// </summary> 
        public static void muteVideosIfCallsMuted(bool value)
        {
            getInstance().muteVideosIfCallsMuted(value);
        }
        
        /// <summary>
        /// Start test screen to test integration.
        /// See <see cref="Appodeal.showTestScreen"/> for resulting triggered event.
        /// </summary> 
        public static void showTestScreen()
        {
            getInstance().showTestScreen();
        }
        
        /// <summary>
        /// Disables data collection for kids apps.
        /// See <see cref="Appodeal.setChildDirectedTreatment"/> for resulting triggered event.
        /// <param name="value">value true to disable data collection for kids apps.</param>
        /// </summary> 
        public static void setChildDirectedTreatment(bool value)
        {
            getInstance().setChildDirectedTreatment(value);
        }
        
        /// <summary>
        /// Destroy cached ad.
        /// See <see cref="Appodeal.destroy"/> for resulting triggered event.
        /// <param name="adTypes">adTypes ad types you want to destroy.</param>
        /// </summary> 
        public static void destroy(int adTypes)
        {
            getInstance().destroy(adTypes);
        }
        
        /// <summary>
        /// Add extra data to Appodeal.
        /// See <see cref="Appodeal.setExtraData"/> for resulting triggered event.
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        /// </summary> 
        public static void setExtraData(string key, bool value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// Add extra data to Appodeal.
        /// See <see cref="Appodeal.setExtraData"/> for resulting triggered event.
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        /// </summary> 
        public static void setExtraData(string key, int value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// Add extra data to Appodeal.
        /// See <see cref="Appodeal.setExtraData"/> for resulting triggered event.
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        /// </summary> 
        public static void setExtraData(string key, double value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// Add extra data to Appodeal.
        /// See <see cref="Appodeal.setExtraData"/> for resulting triggered event.
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        /// </summary> 
        public static void setExtraData(string key, string value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// <para>
        /// Resets extra data value by the provided key.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data#id-[Development]UnitySDK.SetUsersData-Sendextradata"/> for more details.
        /// </summary>
        /// <remarks>Use it to remove an extra data, that was previously set via one of the <see langword="SetExtraData()"/> methods.</remarks>
        /// <param name="key">unique identifier.</param>
        public static void resetExtraData(string key)
        {
            getInstance().resetExtraData(key);
        }
        
        /// <summary>
        /// Get native SDK version
        /// See <see cref="Appodeal.getNativeSDKVersion"/> for resulting triggered event.
        /// </summary> 
        public static string getNativeSDKVersion()
        {
            return getInstance().getVersion();
        }
        
        /// <summary>
        /// Get Unity plugin version
        /// See <see cref="Appodeal.getPluginVersion"/> for resulting triggered event.
        /// </summary> 
        public static string getPluginVersion()
        {
            return APPODEAL_PLUGIN_VERSION;
        }
        
        /// <summary>
        /// Get predicted ecpm for certain ad type.
        /// See <see cref="Appodeal.getPredictedEcpm"/> for resulting triggered event.
        /// <param name="adType">adType type of advertising.</param>
        /// </summary> 
        public static double getPredictedEcpm(int adType)
        {
            return getInstance().getPredictedEcpm(adType);
        }
        
        /// <summary>
        /// Get Unity version
        /// See <see cref="Appodeal.getUnityVersion"/> for resulting triggered event.
        /// </summary> 
        public static string getUnityVersion()
        {
            var unityVersion = Application.unityVersion;
            if (!string.IsNullOrEmpty(unityVersion)) return unityVersion;
            var appId =
                typeof(Application).GetProperty("identifier", BindingFlags.Public | BindingFlags.Static);
            unityVersion = appId != null ? "5.6+" : "5.5-";

            return unityVersion;
        }
        
        /// <summary>
        /// Set use safe area.
        /// See <see cref="Appodeal.setUseSafeArea"/> for resulting triggered event.
        /// </summary> 
        public static void setUseSafeArea(bool value)
        {
            getInstance().setUseSafeArea(value);
        }

        /// <summary>
        /// <para>Sends event data to all connected analytic services such as Firebase, Adjust, AppsFlyer and Facebook.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <remarks>
        /// <para>Event parameter values must be one of the following types:  <see langword="string"/>, <see langword="double"/>, or <see langword="int"/></para>
        /// If event has no params, call the shorten version of this method by passing only name of the event.
        /// </remarks>
        /// <param name="eventName">name of the event.</param>
        /// <param name="eventParams">parameters of the event if any.</param>
        public static void logEvent(string eventName, Dictionary<string, object> eventParams = null)
        {
            getInstance().logEvent(eventName, eventParams);
        }

        /// <summary>
        /// <para>
        /// Validates In-App purchase. (Supported only for <see langword="Android"/> platform)
        /// </para>
        /// See <see href=""/> for more details.
        /// </summary> 
        /// <remarks>If the purchase is valid, this method will also call <see cref="trackInAppPurchase"/> method under the hood.</remarks>
        /// <param name="purchase">object of type PlayStoreInAppPurchase, containing all data about the purchase.</param>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.IInAppPurchaseValidationListener interface.</param>
        public static void validatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener = null)
        {
            getInstance().validatePlayStoreInAppPurchase(purchase, listener);
        }

        /// <summary>
        /// <para>
        /// Validates In-App purchase. (Supported only for <see langword="iOS"/> platform)
        /// </para>
        /// See <see href=""/> for more details.
        /// </summary> 
        /// <remarks>If the purchase is valid, this method will also call <see cref="trackInAppPurchase"/> method under the hood.</remarks>
        /// <param name="purchase">object of type AppStoreInAppPurchase, containing all data about the purchase.</param>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.IInAppPurchaseValidationListener interface.</param>
        public static void validateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener = null)
        {
            getInstance().validateAppStoreInAppPurchase(purchase, listener);
        }

        #region Deprecated methods

        [Obsolete("It will be removed in the next release. Instead use setCustomFilter(string, int) method with UserSettings.USER_AGE as key.", false)]
        public static void setUserAge(int age)
        {
            getInstance().setUserAge(age);
        }

        [Obsolete("It will be removed in the next release. Instead use setCustomFilter(string, int) method with UserSettings.USER_GENDER as key.", false)]
        public static void setUserGender(UserSettings.Gender gender)
        {
            getInstance().setUserGender(gender);
        }

        [Obsolete("It will be removed in the next release.", false)]
        public static void setSharedAdsInstanceAcrossActivities(bool sharedAdsInstanceAcrossActivities)
        {
            getInstance().setSharedAdsInstanceAcrossActivities(sharedAdsInstanceAcrossActivities);
        }

        [Obsolete("It will be removed in the next release. Use UpdateGdprConsent and UpdateCcpaConsent methods instead.", false)]
        public static void updateConsent(bool hasConsent)
        {
            getInstance().updateConsent(hasConsent);
        }

        [Obsolete("It will be removed in the next release. Check documentation for the new signature.", false)]
        public static void initialize(string appKey, int adTypes)
        {
            getInstance().initialize(appKey, adTypes);
        }

        [Obsolete("It will be removed in the next release. Check documentation for the new signature.", false)]
        public static void initialize(string appKey, int adTypes, bool hasConsent)
        {
            getInstance().initialize(appKey, adTypes, hasConsent);
        }

        [Obsolete("It will be removed in the next release. Check documentation for the new signature.", false)]
        public static void initialize(string appKey, int adTypes, Consent consent)
        {
            getInstance().initialize(appKey, adTypes, consent);
        }

        [Obsolete("It was removed from iOS SDK, thus cannot be used anymore. It will be removed in the next release of Appodeal Unity Plugin.", true)]
        public static void setBannerBackground(bool enabled) { }
        
        #endregion
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class ExtraData
    {
        public const string APPSFLYER_ID = "appsflyer_id";
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class UserSettings
    {
        public const string USER_AGE = "appodeal_user_age";
        public const string USER_GENDER = "appodeal_user_gender";

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public enum Gender
        {
            OTHER,
            MALE,
            FEMALE
        }
    }

    /// <summary>
    /// <para>AppStoreInAppPurchase Unity API for developers, including documentation.</para>
    /// See <see href=""/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AppStoreInAppPurchase : IAppStoreInAppPurchase
    {
        /// <summary>
        /// Provides access to a native object that implements IAppStoreInAppPurchase interface.
        /// </summary>
        public IAppStoreInAppPurchase NativeInAppPurchase { get; }

        /// <summary>
        /// Public constructor of the <see langword="AppStoreInAppPurchase"/> class.
        /// </summary>
        /// <param name="purchase">class which implements AppodealStack.Monetization.Common.IAppStoreInAppPurchase interface.</param>
        public AppStoreInAppPurchase(IAppStoreInAppPurchase purchase)
        {
            NativeInAppPurchase = purchase.NativeInAppPurchase;
        }

        /// <summary>
        /// <para>Gets the purchase type.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Type of the purchase as AppStorePurchaseType object.</returns>
        public Appodeal.AppStorePurchaseType getPurchaseType()
        {
            return NativeInAppPurchase.getPurchaseType();
        }

        /// <summary>
        /// <para>Gets an id of the purchased product.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Product Id as string.</returns>
        public string getProductId()
        {
            return NativeInAppPurchase.getProductId();
        }

        /// <summary>
        /// <para>Gets the transaction id of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Id of the transaction as string.</returns>
        public string getTransactionId()
        {
            return NativeInAppPurchase.getTransactionId();
        }

        /// <summary>
        /// <para>Gets the price of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Price as string.</returns>
        public string getPrice()
        {
            return NativeInAppPurchase.getPrice();
        }

        /// <summary>
        /// <para>Gets the currency of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Currency as string.</returns>
        public string getCurrency()
        {
            return NativeInAppPurchase.getCurrency();
        }

        /// <summary>
        /// <para>Gets the additional parameters of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Additional parameters as string.</returns>
        public string getAdditionalParameters()
        {
            return NativeInAppPurchase.getAdditionalParameters();
        }

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="AppStoreInAppPurchase"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IAppStoreInAppPurchaseBuilder _appStoreInAppPurchaseBuilder;

            private IAppStoreInAppPurchaseBuilder getBuilderInstance()
            {
                return _appStoreInAppPurchaseBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="purchaseType">type of the purchase.</param>
            public Builder(Appodeal.AppStorePurchaseType purchaseType)
            {
                 _appStoreInAppPurchaseBuilder = AppodealAdsClientFactory.GetAppStoreInAppPurchaseBuilder(purchaseType);
            }

            /// <summary>
            /// Builds the AppStoreInAppPurchase object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="AppStoreInAppPurchase"/>.</returns>
            public AppStoreInAppPurchase build()
            {
                return new AppStoreInAppPurchase(getBuilderInstance().build());
            }

            /// <summary>
            /// Sets an id of the purchased product.
            /// </summary>
            /// <param name="productId">product id as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withProductId(string productId)
            {
                getBuilderInstance().withProductId(productId);
                return this;
            }

            /// <summary>
            /// Sets the transaction id of the purchase.
            /// </summary>
            /// <param name="transactionId">id of the transaction as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withTransactionId(string transactionId)
            {
                getBuilderInstance().withTransactionId(transactionId);
                return this;
            }

            /// <summary>
            /// Sets the price of the purchase.
            /// </summary>
            /// <param name="price">purchase price as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withPrice(string price)
            {
                getBuilderInstance().withPrice(price);
                return this;
            }

            /// <summary>
            /// Sets the currency of the purchase.
            /// </summary>
            /// <param name="currency">purchase currency as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withCurrency(string currency)
            {
                getBuilderInstance().withCurrency(currency);
                return this;
            }

            /// <summary>
            /// Sets the additional parameters of the purchase.
            /// </summary>
            /// <param name="additionalParameters">additional parameters as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withAdditionalParameters(Dictionary<string, string> additionalParameters)
            {
                getBuilderInstance().withAdditionalParameters(additionalParameters);
                return this;
            }
        }
    }

    /// <summary>
    /// <para>PlayStoreInAppPurchase Unity API for developers, including documentation.</para>
    /// See <see href=""/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PlayStoreInAppPurchase : IPlayStoreInAppPurchase
    {
        /// <summary>
        /// Provides access to a native object that implements IPlayStoreInAppPurchase interface.
        /// </summary>
        public IPlayStoreInAppPurchase NativeInAppPurchase { get; }

        /// <summary>
        /// Public constructor of the <see langword="PlayStoreInAppPurchase"/> class.
        /// </summary>
        /// <param name="purchase">class which implements AppodealStack.Monetization.Common.IPlayStoreInAppPurchase interface.</param>
        public PlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase)
        {
            NativeInAppPurchase = purchase.NativeInAppPurchase;
        }

        /// <summary>
        /// <para>Gets the purchase type.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Type of the purchase as PlayStorePurchaseType object.</returns>
        public Appodeal.PlayStorePurchaseType getPurchaseType()
        {
            return NativeInAppPurchase.getPurchaseType();
        }

        /// <summary>
        /// <para>Gets the public key of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Public key as string.</returns>
        public string getPublicKey()
        {
            return NativeInAppPurchase.getPublicKey();
        }

        /// <summary>
        /// <para>Gets the signature of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Signature as string.</returns>
        public string getSignature()
        {
            return NativeInAppPurchase.getSignature();
        }

        /// <summary>
        /// <para>Gets the purchase data of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Purchase data as string.</returns>
        public string getPurchaseData()
        {
            return NativeInAppPurchase.getPurchaseData();
        }

        /// <summary>
        /// <para>Gets the price of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Price as string.</returns>
        public string getPrice()
        {
            return NativeInAppPurchase.getPrice();
        }

        /// <summary>
        /// <para>Gets the currency of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Currency as string.</returns>
        public string getCurrency()
        {
            return NativeInAppPurchase.getCurrency();
        }

        /// <summary>
        /// <para>Gets the additional parameters of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Additional parameters as string.</returns>
        public string getAdditionalParameters()
        {
            return NativeInAppPurchase.getAdditionalParameters();
        }

        /// <summary>
        /// <para>Gets the SKU of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>SKU as string.</returns>
        public string getSku()
        {
            return NativeInAppPurchase.getSku();
        }

        /// <summary>
        /// <para>Gets the order id of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Order id as string.</returns>
        public string getOrderId()
        {
            return NativeInAppPurchase.getOrderId();
        }

        /// <summary>
        /// <para>Gets the token of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Purchase token as string.</returns>
        public string getPurchaseToken()
        {
            return NativeInAppPurchase.getPurchaseToken();
        }

        /// <summary>
        /// <para>Gets the timestamp of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Purchase timestamp as string.</returns>
        public long getPurchaseTimestamp()
        {
            return NativeInAppPurchase.getPurchaseTimestamp();
        }

        /// <summary>
        /// <para>Gets the developer payload of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Developer payload as string.</returns>
        public string getDeveloperPayload()
        {
            return NativeInAppPurchase.getDeveloperPayload();
        }

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="PlayStoreInAppPurchase"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IPlayStoreInAppPurchaseBuilder _playStoreInAppPurchaseBuilder;

            private IPlayStoreInAppPurchaseBuilder getBuilderInstance()
            {
                return _playStoreInAppPurchaseBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="purchaseType">type of the purchase.</param>
            public Builder(Appodeal.PlayStorePurchaseType purchaseType)
            {
                 _playStoreInAppPurchaseBuilder = AppodealAdsClientFactory.GetPlayStoreInAppPurchaseBuilder(purchaseType);
            }

            /// <summary>
            /// Builds the PlayStoreInAppPurchase object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="PlayStoreInAppPurchase"/>.</returns>
            public PlayStoreInAppPurchase build()
            {
                return new PlayStoreInAppPurchase(getBuilderInstance().build());
            }

            /// <summary>
            /// Sets the public key of the purchase.
            /// </summary>
            /// <param name="publicKey">public key as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withPublicKey(string publicKey)
            {
                getBuilderInstance().withPublicKey(publicKey);
                return this;
            }

            /// <summary>
            /// Sets the signature of the purchase.
            /// </summary>
            /// <param name="signature">purchase signature as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withSignature(string signature)
            {
                getBuilderInstance().withSignature(signature);
                return this;
            }

            /// <summary>
            /// Sets the purchase data.
            /// </summary>
            /// <param name="purchaseData">purchase data as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withPurchaseData(string purchaseData)
            {
                getBuilderInstance().withPurchaseData(purchaseData);
                return this;
            }

            /// <summary>
            /// Sets the price of the purchase.
            /// </summary>
            /// <param name="price">purchase price as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withPrice(string price)
            {
                getBuilderInstance().withPrice(price);
                return this;
            }

            /// <summary>
            /// Sets the currency of the purchase.
            /// </summary>
            /// <param name="currency">purchase currency as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withCurrency(string currency)
            {
                getBuilderInstance().withCurrency(currency);
                return this;
            }

            /// <summary>
            /// Sets the SKU of the purchase.
            /// </summary>
            /// <param name="sku">purchase SKU as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withSku(string sku)
            {
                getBuilderInstance().withSku(sku);
                return this;
            }

            /// <summary>
            /// Sets the order id of the purchase.
            /// </summary>
            /// <param name="orderId">order id as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withOrderId(string orderId)
            {
                getBuilderInstance().withOrderId(orderId);
                return this;
            }

            /// <summary>
            /// Sets the token of the purchase.
            /// </summary>
            /// <param name="purchaseToken">Purchase token as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withPurchaseToken(string purchaseToken)
            {
                getBuilderInstance().withPurchaseToken(purchaseToken);
                return this;
            }

            /// <summary>
            /// Sets the timestamp of the purchase.
            /// </summary>
            /// <param name="purchaseTimestamp">purchase timestamp as long.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withPurchaseTimestamp(long purchaseTimestamp)
            {
                getBuilderInstance().withPurchaseTimestamp(purchaseTimestamp);
                return this;
            }

            /// <summary>
            /// Sets the additional parameters of the purchase.
            /// </summary>
            /// <param name="additionalParameters">additional parameters as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withAdditionalParameters(Dictionary<string, string> additionalParameters)
            {
                getBuilderInstance().withAdditionalParameters(additionalParameters);
                return this;
            }

            /// <summary>
            /// Sets the developer payload of the purchase.
            /// </summary>
            /// <param name="developerPayload">developer payload as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withDeveloperPayload(string developerPayload)
            {
                getBuilderInstance().withDeveloperPayload(developerPayload);
                return this;
            }
        }
    }
}
