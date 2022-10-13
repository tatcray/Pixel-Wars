#if UNITY_IPHONE
using System;
using System.Linq;
using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.iOS
{    
    public class IosAppStoreInAppPurchase : IAppStoreInAppPurchase
    {
        public IAppStoreInAppPurchase NativeInAppPurchase { get; }
        private Appodeal.AppStorePurchaseType PurchaseType { get; }

        public string Price { get; set; } = String.Empty;
        public string Currency { get; set; } = String.Empty;
        public string ProductId { get; set; } = String.Empty;
        public string TransactionId { get; set; } = String.Empty;
        public Dictionary<string, string> AdditionalParameters { get; set; }

        public IosAppStoreInAppPurchase(Appodeal.AppStorePurchaseType purchaseType)
        {
            PurchaseType = purchaseType;
            NativeInAppPurchase = this;
        }

        public Appodeal.AppStorePurchaseType getPurchaseType()
        {
            return PurchaseType;
        }

        public string getProductId()
        {
            return ProductId;
        }

        public string getTransactionId()
        {
            return TransactionId;
        }

        public string getPrice()
        {
            return Price;
        }

        public string getCurrency()
        {
            return Currency;
        }

        public string getAdditionalParameters()
        {
            return AdditionalParameters?.Aggregate("", (current, keyValues) => current + (keyValues.Key + "=" + keyValues.Value + ",")).TrimEnd(',') ?? String.Empty;
        }
    }
}
#endif
