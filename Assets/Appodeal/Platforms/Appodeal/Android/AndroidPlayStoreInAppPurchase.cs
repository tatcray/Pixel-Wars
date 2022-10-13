#if UNITY_ANDROID
using System;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Android
{    
    public class AndroidPlayStoreInAppPurchase : IPlayStoreInAppPurchase
    {
        public IPlayStoreInAppPurchase NativeInAppPurchase { get; }

        private readonly AndroidJavaObject _inAppPurchase;

        public AndroidPlayStoreInAppPurchase (AndroidJavaObject inAppPurchase)
        {
            _inAppPurchase = inAppPurchase;
            NativeInAppPurchase = this;
        }

        public AndroidJavaObject getInAppPurchase()
        {
            return _inAppPurchase;
        }

        public Appodeal.PlayStorePurchaseType getPurchaseType()
        {
            string type = getInAppPurchase().Call<AndroidJavaObject>("getType").Call<string>("toString");

            switch (type)
            {
                case "Subs": return Appodeal.PlayStorePurchaseType.Subs;
                case "InApp": return Appodeal.PlayStorePurchaseType.InApp;
                default: throw new ArgumentOutOfRangeException(nameof(Appodeal.PlayStorePurchaseType), type, null);
            }
        }

        public string getPublicKey()
        {
            return getInAppPurchase().Call<string>("getPublicKey");
        }

        public string getSignature()
        {
            return getInAppPurchase().Call<string>("getSignature");
        }

        public string getPurchaseData()
        {
            return getInAppPurchase().Call<string>("getPurchaseData");
        }

        public string getPrice()
        {
            return getInAppPurchase().Call<string>("getPrice");
        }

        public string getCurrency()
        {
            return getInAppPurchase().Call<string>("getCurrency");
        }

        public string getSku()
        {
            return getInAppPurchase().Call<string>("getSku");
        }

        public string getOrderId()
        {
            return getInAppPurchase().Call<string>("getOrderId");
        }

        public string getPurchaseToken()
        {
            return getInAppPurchase().Call<string>("getPurchaseToken");
        }

        public long getPurchaseTimestamp()
        {
            return getInAppPurchase().Call<long>("getPurchaseTimestamp");
        }

        public string getAdditionalParameters()
        {
            return getInAppPurchase().Call<AndroidJavaObject>("getAdditionalParameters").Call<string>("toString");
        }

        public string getDeveloperPayload()
        {
            return getInAppPurchase().Call<string>("getDeveloperPayload");
        }
    }
}
#endif
