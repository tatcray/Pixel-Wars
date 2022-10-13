using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity
{
    internal static class AppodealAdsClientFactory
    {
        internal static IAppodealAdsClient GetAppodealAdsClient()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new Android.AndroidAppodealClient();
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new iOS.AppodealAdsClient();
#else
            return new Dummy.DummyClient();
#endif
        }

        internal static IPlayStoreInAppPurchaseBuilder GetPlayStoreInAppPurchaseBuilder(Appodeal.PlayStorePurchaseType purchaseType)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new Android.AndroidPlayStoreInAppPurchaseBuilder(purchaseType);
#elif UNITY_IPHONE && !UNITY_EDITOR
            return null;
#else
            return new Dummy.DummyPlayStoreInAppPurchaseBuilder();
#endif
        }

        internal static IAppStoreInAppPurchaseBuilder GetAppStoreInAppPurchaseBuilder(Appodeal.AppStorePurchaseType purchaseType)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return null;
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new iOS.IosAppStoreInAppPurchaseBuilder(purchaseType);
#else
            return new Dummy.DummyAppStoreInAppPurchaseBuilder();
#endif
        }
    }
}
