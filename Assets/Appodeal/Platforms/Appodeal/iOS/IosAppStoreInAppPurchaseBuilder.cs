#if UNITY_IPHONE
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.iOS
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class IosAppStoreInAppPurchaseBuilder : IAppStoreInAppPurchaseBuilder
    {
        private readonly IosAppStoreInAppPurchase _purchase;

        public IosAppStoreInAppPurchaseBuilder(Appodeal.AppStorePurchaseType purchaseType)
        {
            _purchase = new IosAppStoreInAppPurchase(purchaseType);
        }

        public IAppStoreInAppPurchase build()
        {
            return _purchase;
        }

        public void withProductId(string productId)
        {
            _purchase.ProductId = productId;
        }

        public void withTransactionId(string transactionId)
        {
            _purchase.TransactionId = transactionId;
        }

        public void withPrice(string price)
        {
            _purchase.Price = price;
        }

        public void withCurrency(string currency)
        {
            _purchase.Currency = currency;
        }

        public void withAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            _purchase.AdditionalParameters = additionalParameters;
        }
    }
}
#endif
