#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Android
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class AndroidPlayStoreInAppPurchaseBuilder : IPlayStoreInAppPurchaseBuilder
    {
        private readonly AndroidJavaObject _inAppPurchaseBuilder;
        private AndroidJavaObject _inAppPurchase;

        public AndroidPlayStoreInAppPurchaseBuilder(Appodeal.PlayStorePurchaseType purchaseType)
        {
            switch (purchaseType)
            {
                case Appodeal.PlayStorePurchaseType.Subs:
                    _inAppPurchaseBuilder = new AndroidJavaClass("com.appodeal.ads.inapp.InAppPurchase").CallStatic<AndroidJavaObject>("newSubscriptionBuilder");
                    break;
                case Appodeal.PlayStorePurchaseType.InApp:
                    _inAppPurchaseBuilder = new AndroidJavaClass("com.appodeal.ads.inapp.InAppPurchase").CallStatic<AndroidJavaObject>("newInAppBuilder");
                    break;
            }
        }

        private AndroidJavaObject getBuilder()
        {
            return _inAppPurchaseBuilder;
        }

        public IPlayStoreInAppPurchase build()
        {
            _inAppPurchase = getBuilder().Call<AndroidJavaObject>("build");
            return new AndroidPlayStoreInAppPurchase(_inAppPurchase);
        }

        public void withPublicKey(string publicKey)
        {
            getBuilder().Call<AndroidJavaObject>("withPublicKey", publicKey);
        }

        public void withSignature(string signature)
        {
            getBuilder().Call<AndroidJavaObject>("withSignature", signature);
        }

        public void withPurchaseData(string purchaseData)
        {
            getBuilder().Call<AndroidJavaObject>("withPurchaseData", purchaseData);
        }

        public void withPrice(string price)
        {
            getBuilder().Call<AndroidJavaObject>("withPrice", price);
        }

        public void withCurrency(string currency)
        {
            getBuilder().Call<AndroidJavaObject>("withCurrency", currency);
        }

        public void withSku(string sku)
        {
            getBuilder().Call<AndroidJavaObject>("withSku", sku);
        }

        public void withOrderId(string orderId)
        {
            getBuilder().Call<AndroidJavaObject>("withOrderId", orderId);
        }

        public void withDeveloperPayload(string developerPayload)
        {
            getBuilder().Call<AndroidJavaObject>("withDeveloperPayload", developerPayload);
        }

        public void withPurchaseToken(string purchaseToken)
        {
            getBuilder().Call<AndroidJavaObject>("withPurchaseToken", purchaseToken);
        }

        public void withPurchaseTimestamp(long purchaseTimestamp)
        {
            getBuilder().Call<AndroidJavaObject>("withPurchaseTimestamp", purchaseTimestamp);
        }

        public void withAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            var map = new AndroidJavaObject("java.util.HashMap");
            foreach (var entry in additionalParameters)
            {
                map.Call<AndroidJavaObject>("put", entry.Key, Helper.getJavaObject(entry.Value));
            }

            getBuilder().Call<AndroidJavaObject>("withAdditionalParams", map);
        }
    }
}
#endif
