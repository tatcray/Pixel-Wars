using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using ConsentManager.Common;
using UnityEngine;
using UnityEngine.UI;

namespace ConsentManager.ConsentManagerDemo.Scripts
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public class AppodealDemo : MonoBehaviour, IAppodealInitializationListener, IInAppPurchaseValidationListener,
                                IBannerAdListener, IInterstitialAdListener, IRewardedVideoAdListener, IMrecAdListener,
                                IConsentFormListener, IConsentInfoUpdateListener
    {
        #region Constants

        private const string CACHE_INTERSTITIAL = "CACHE INTERSTITIAL";
        private const string SHOW_INTERSTITIAL = "SHOW INTERSTITIAL";
        private const string CACHE_REWARDED_VIDEO = "CACHE REWARDED VIDEO";

        #endregion

        #region UI

        [SerializeField] public Toggle tgTesting;
        [SerializeField] public Toggle tgLogging;
        [SerializeField] public Button btnShowInterstitial;
        [SerializeField] public Button btnShowRewardedVideo;
        [SerializeField] public GameObject consentManagerPanel;
        [SerializeField] public GameObject appodealPanel;

        #endregion

        #region Application keys

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE
        public static string appKey = "";
#elif UNITY_ANDROID
        public static string appKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";
#elif UNITY_IPHONE
        public static string appKey = "466de0d625e01e8811c588588a42a55970bc7c132649eede";
#else
	    public static string appKey = "";
#endif

        #endregion

        private Consent currentConsent;
        private ConsentForm consentForm;
        private ConsentManager consentManager;

        private void Start()
        {
            consentManagerPanel.gameObject.SetActive(true);
            appodealPanel.gameObject.SetActive(false);

            btnShowInterstitial.GetComponentInChildren<Text>().text = CACHE_INTERSTITIAL;
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = CACHE_REWARDED_VIDEO;

            consentManager = ConsentManager.getInstance();
            consentManager.setStorage(ConsentManager.Storage.SHARED_PREFERENCE);
        }

        private void OnDestroy()
        {
            Appodeal.destroy(Appodeal.BANNER);
        }

        public void RequestConsentInfoUpdate()
        {
            consentManager?.requestConsentInfoUpdate(appKey, this);
        }

        public void SetCustomVendor()
        {
            var customVendor = new Vendor.Builder(
                    "Appodeal Test",
                    "com.appodeal.test",
                    "https://customvendor.com")
                .setPurposeIds(new List<int> {100, 200, 300})
                .setFeatureIds(new List<int> {400, 500, 600})
                .setLegitimateInterestPurposeIds(new List<int> {700, 800, 900})
                .build();

            consentManager?.setCustomVendor(customVendor);

            var vendor = consentManager?.getCustomVendor("com.appodeal.test");
            if (vendor == null) return;
            Debug.Log("Vendor getName: " + vendor.getName());
            Debug.Log("Vendor getBundle: " + vendor.getBundle());
            Debug.Log("Vendor getPolicyUrl: " + vendor.getPolicyUrl());
            foreach (var purposeId in vendor.getPurposeIds())
            {
                Debug.Log("Vendor getPurposeIds: " + purposeId);
            }

            foreach (var featureId in vendor.getFeatureIds())
            {
                Debug.Log("Vendor getFeatureIds: " + featureId);
            }

            foreach (var legitimateInterestPurposeId in vendor.getLegitimateInterestPurposeIds())
            {
                Debug.Log("Vendor getLegitimateInterestPurposeIds: " + legitimateInterestPurposeId);
            }
        }

        public void ShouldShowForm()
        {
            Debug.Log("shouldShowConsentDialog: " + consentManager.shouldShowConsentDialog());
        }

        public void GetConsentZone()
        {
            Debug.Log("getConsentZone: " + consentManager.getConsentZone());
        }

        public void GetConsentStatus()
        {
            Debug.Log("getConsentStatus: " + consentManager.getConsentStatus());
        }

        public void LoadConsentForm()
        {
            consentForm = ConsentForm.GetInstance(this);
            consentForm?.load();
        }

        public void IsLoadedConsentForm()
        {
            if (consentForm != null)
            {
                Debug.Log("isLoadedConsentForm:  " + consentForm.isLoaded());
            }
        }

        public void ShowFormAsActivity()
        {
            if (consentForm != null)
            {
                consentForm.show();
            }
            else
            {
                Debug.Log("showForm - false");
            }
        }

        public void ShowFormAsDialog()
        {
            if (consentForm != null)
            {
                consentForm.show();
            }
            else
            {
                Debug.Log("showForm - false");
            }
        }

        public void PrintIABString()
        {
            Debug.Log("Consent IAB String is: " + consentManager.getConsent().getIabConsentString());
        }

        public void PrintCurrentConsent()
        {
            if (consentManager.getConsent() == null) return;
            Debug.Log("consent.hasConsentForVendor() - " + consentManager.getConsent().hasConsentForVendor("com.appodeal.test"));
            Debug.Log("consent.getStatus() - " + consentManager.getConsent().getStatus());
            Debug.Log("consent.getZone() - " + consentManager.getConsent().getZone());
        }

        public void PrintAuthorizationStatus()
        {
            if (consentManager.getConsent() == null) return;
            Debug.Log($"AuthorizationStatus - {consentManager.getConsent().getAuthorizationStatus()} ");
        }

        public void ShowAppodealLogic()
        {
            consentManagerPanel.SetActive(false);
            appodealPanel.SetActive(true);
        }

        public void Initialize()
        {
            InitWithConsent(currentConsent != null);
        }

        public void InitWithConsent(bool isConsent)
        {
            Appodeal.setLogLevel(tgLogging.isOn ? Appodeal.LogLevel.Verbose : Appodeal.LogLevel.None);
            Appodeal.setTesting(tgTesting.isOn);
            Appodeal.setUseSafeArea(true);

            Appodeal.setUserId("1");
            Appodeal.setCustomFilter(UserSettings.USER_AGE, 18);
            Appodeal.setCustomFilter(UserSettings.USER_GENDER, (int) UserSettings.Gender.MALE);

            Appodeal.setExtraData("testKey", "testValue");
            Appodeal.resetExtraData("testKey");

            Appodeal.setSmartBanners(true);
            Appodeal.setBannerAnimation(false);
            Appodeal.setTabletBanners(false);
            Appodeal.setBannerRotation(-90, 110);

            Appodeal.disableLocationPermissionCheck();
            Appodeal.setChildDirectedTreatment(false);
            Appodeal.muteVideosIfCallsMuted(true);

            Appodeal.setTriggerOnLoadedOnPrecache(Appodeal.INTERSTITIAL, true);

            Appodeal.disableNetwork(AppodealNetworks.VUNGLE);
            Appodeal.disableNetwork(AppodealNetworks.YANDEX, Appodeal.MREC);

            Appodeal.setAutoCache(Appodeal.INTERSTITIAL, false);
            Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);

            Appodeal.setBannerCallbacks(this);
            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);
            Appodeal.setMrecCallbacks(this);

            Appodeal.setCustomFilter("newBoolean", true);
            Appodeal.setCustomFilter("newInt", 1234567890);
            Appodeal.setCustomFilter("newDouble", 123.123456789);
            Appodeal.setCustomFilter("newString", "newStringFromSDK");

            if (isConsent)
            {
                Appodeal.updateConsent(currentConsent);
            }
            else
            {
                Appodeal.updateCcpaConsent(Appodeal.CcpaUserConsent.OptOut);
                Appodeal.updateGdprConsent(Appodeal.GdprUserConsent.NonPersonalized);
            }

            int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO | Appodeal.MREC;
            Appodeal.initialize(appKey, adTypes, (IAppodealInitializationListener) this);
        }

        public void ShowInterstitial()
        {
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && Appodeal.canShow(Appodeal.INTERSTITIAL, "default") && !Appodeal.isPrecache(Appodeal.INTERSTITIAL))
            {
                Appodeal.show(Appodeal.INTERSTITIAL);
            }
            else
            {
                Appodeal.cache(Appodeal.INTERSTITIAL);
            }
        }

        public void ShowRewardedVideo()
        {
            if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && Appodeal.canShow(Appodeal.REWARDED_VIDEO, "default"))
            {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
            else
            {
                Appodeal.cache(Appodeal.REWARDED_VIDEO);
            }
        }

        public void ShowBannerBottom()
        {
            Appodeal.show(Appodeal.BANNER_BOTTOM, "default");
        }

        public void ShowBannerTop()
        {
            Appodeal.show(Appodeal.BANNER_TOP, "default");
        }

        public void HideBanner()
        {
            Appodeal.hide(Appodeal.BANNER);
        }

        public void ShowBannerView()
        {
            Appodeal.showBannerView(Appodeal.BANNER_BOTTOM, Appodeal.BANNER_HORIZONTAL_CENTER, "default");
        }

        public void HideBannerView()
        {
            Appodeal.hideBannerView();
        }

        public void ShowMrecView()
        {
            Appodeal.showMrecView(Appodeal.BANNER_TOP, Appodeal.BANNER_HORIZONTAL_CENTER, "default");
        }

        public void HideMrecView()
        {
            Appodeal.hideMrecView();
        }

        public void ShowBannerLeft()
        {
            Appodeal.show(Appodeal.BANNER_LEFT);
        }

        public void ShowBannerRight()
        {
            Appodeal.show(Appodeal.BANNER_RIGHT);
        }

        #region AppodealInitializeListener

        public void onInitializationFinished(List<string> errors)
        {
            string output = errors == null ? string.Empty : string.Join(", ", errors);
            Debug.Log($"onInitializationFinished(errors:[{output}])");

            Debug.Log($"isAutoCacheEnabled() for banner: {Appodeal.isAutoCacheEnabled(Appodeal.BANNER)}");
            Debug.Log($"isInitialized() for banner: {Appodeal.isInitialized(Appodeal.BANNER)}");
            Debug.Log($"isSmartBannersEnabled(): {Appodeal.isSmartBannersEnabled()}");
            Debug.Log($"getUserId(): {Appodeal.getUserId()}");
            Debug.Log($"getSegmentId(): {Appodeal.getSegmentId()}");
            Debug.Log($"getRewardParameters(): {Appodeal.getRewardParameters()}");
            Debug.Log($"getNativeSDKVersion(): {Appodeal.getNativeSDKVersion()}");

            var networksList = Appodeal.getNetworks(Appodeal.REWARDED_VIDEO);
            output = networksList == null ? string.Empty : string.Join(", ", (networksList.ToArray()));
            Debug.Log($"getNetworks() for RV: {output}");

#if UNITY_ANDROID
            var additionalParams = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

            var purchase = new PlayStoreInAppPurchase.Builder(Appodeal.PlayStorePurchaseType.Subs)
                .withAdditionalParameters(additionalParams)
                .withPurchaseTimestamp(793668600)
                .withDeveloperPayload("payload")
                .withPurchaseToken("token")
                .withPurchaseData("data")
                .withPublicKey("key")
                .withSignature("signature")
                .withCurrency("USD")
                .withOrderId("orderId")
                .withPrice("1.99")
                .withSku("sku")
                .build();

            Appodeal.validatePlayStoreInAppPurchase(purchase, this);
#elif UNITY_IOS
            var additionalParams = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

            var purchase = new AppStoreInAppPurchase.Builder(Appodeal.AppStorePurchaseType.Consumable)
                .withAdditionalParameters(additionalParams)
                .withTransactionId("transactionId")
                .withProductId("productId")
                .withCurrency("USD")
                .withPrice("2.89")
                .build();

            Appodeal.validateAppStoreInAppPurchase(purchase, this);
#endif

            Appodeal.logEvent("test_event", new Dictionary<string, object> { { "test_key_1", 42 }, { "test_key_2", "test_value" } });
        }

        #endregion

        #region InAppPurchaseValidationListener

        public void onInAppPurchaseValidationSucceeded(string json)
        {
            Debug.Log($"onInAppPurchaseValidationSucceeded(string json:\n{json})");
        }

        public void onInAppPurchaseValidationFailed(string json)
        {
            Debug.Log($"onInAppPurchaseValidationFailed(string json:\n{json})");
        }

        #endregion

        #region ConsentFormListener

        public void onConsentFormLoaded()
        {
            Debug.Log("ConsentFormListener - onConsentFormLoaded");
        }

        public void onConsentFormError(ConsentManagerException exception)
        {
            Debug.Log($"ConsentFormListener - onConsentFormError, reason - {exception.getReason()}");
        }

        public void onConsentFormOpened()
        {
            Debug.Log("ConsentFormListener - onConsentFormOpened");
        }

        public void onConsentFormClosed(Consent consent)
        {
            currentConsent = consent;
            Debug.Log($"ConsentFormListener - onConsentFormClosed, consentStatus - {consent.getStatus()}");
        }

        #endregion

        #region ConsentInfoUpdateListener

        public void onConsentInfoUpdated(Consent consent)
        {
            currentConsent = consent;
            Debug.Log("onConsentInfoUpdated");
        }

        public void onFailedToUpdateConsentInfo(ConsentManagerException error)
        {
            Debug.Log($"onFailedToUpdateConsentInfo");

            if (error == null) return;
            Debug.Log($"onFailedToUpdateConsentInfo Reason: {error.getReason()}");

            switch (error.getCode())
            {
                case 0:
                    Debug.Log("onFailedToUpdateConsentInfo - UNKNOWN");
                    break;
                case 1:
                    Debug.Log(
                        "onFailedToUpdateConsentInfo - INTERNAL - Error on SDK side. Includes JS-bridge or encoding/decoding errors");
                    break;
                case 2:
                    Debug.Log("onFailedToUpdateConsentInfo - NETWORKING - HTTP errors, parse request/response ");
                    break;
                case 3:
                    Debug.Log("onFailedToUpdateConsentInfo - INCONSISTENT - Incorrect SDK API usage");
                    break;
            }
        }

        #endregion

        #region Banner callback handlers

        public void onBannerLoaded(int height, bool precache)
        {
            Debug.Log("onBannerLoaded");
            Debug.Log($"Banner height - {height}");
            Debug.Log($"Banner precache - {precache}");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.BANNER)}");
        }

        public void onBannerFailedToLoad()
        {
            Debug.Log("onBannerFailedToLoad");
        }

        public void onBannerShown()
        {
            Debug.Log("onBannerShown");
        }

        public void onBannerShowFailed()
        {
            Debug.Log("onBannerShowFailed");
        }

        public void onBannerClicked()
        {
            Debug.Log("onBannerClicked");
        }

        public void onBannerExpired()
        {
            Debug.Log("onBannerExpired");
        }

        #endregion

        #region Interstitial callback handlers

        public void onInterstitialLoaded(bool isPrecache)
        {
            if (!isPrecache)
            {
                btnShowInterstitial.GetComponentInChildren<Text>().text = SHOW_INTERSTITIAL;
            }
            else
            {
                Debug.Log("Appodeal. Interstitial loaded. isPrecache - true");
            }

            Debug.Log("onInterstitialLoaded");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.INTERSTITIAL)}");
        }

        public void onInterstitialFailedToLoad()
        {
            Debug.Log("onInterstitialFailedToLoad");
        }

        public void onInterstitialShowFailed()
        {
            Debug.Log("onInterstitialShowFailed");
        }

        public void onInterstitialShown()
        {
            Debug.Log("onInterstitialShown");
        }

        public void onInterstitialClosed()
        {
            btnShowInterstitial.GetComponentInChildren<Text>().text = CACHE_INTERSTITIAL;
            Debug.Log("onInterstitialClosed");
        }

        public void onInterstitialClicked()
        {
            Debug.Log("onInterstitialClicked");
        }

        public void onInterstitialExpired()
        {
            Debug.Log("onInterstitialExpired");
        }

        #endregion

        #region Rewarded Video callback handlers

        public void onRewardedVideoLoaded(bool isPrecache)
        {
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "SHOW REWARDED VIDEO";
            Debug.Log("onRewardedVideoLoaded");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.REWARDED_VIDEO)}");
        }

        public void onRewardedVideoFailedToLoad()
        {
            Debug.Log("onRewardedVideoFailedToLoad");
        }

        public void onRewardedVideoShowFailed()
        {
            Debug.Log("onRewardedVideoShowFailed");
        }

        public void onRewardedVideoShown()
        {
            Debug.Log("onRewardedVideoShown");
        }

        public void onRewardedVideoClosed(bool finished)
        {
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "CACHE REWARDED VIDEO";
            Debug.Log($"onRewardedVideoClosed. Finished - {finished}");
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            Debug.Log("onRewardedVideoFinished. Reward: " + amount + " " + name);
        }

        public void onRewardedVideoExpired()
        {
            Debug.Log("onRewardedVideoExpired");
        }

        public void onRewardedVideoClicked()
        {
            Debug.Log("onRewardedVideoClicked");
        }

        #endregion

        #region Mrec callback handlers

        public void onMrecLoaded(bool precache)
        {
            Debug.Log($"onMrecLoaded. Precache - {precache}");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.MREC)}");
        }

        public void onMrecFailedToLoad()
        {
            Debug.Log("onMrecFailedToLoad");
        }

        public void onMrecShown()
        {
            Debug.Log("onMrecShown");
        }

        public void onMrecShowFailed()
        {
            Debug.Log("onMrecShowFailed");
        }

        public void onMrecClicked()
        {
            Debug.Log("onMrecClicked");
        }

        public void onMrecExpired()
        {
            Debug.Log("onMrecExpired");
        }

        #endregion
    }
}
