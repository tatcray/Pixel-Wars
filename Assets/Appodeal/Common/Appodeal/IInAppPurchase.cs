using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;

namespace AppodealAds.Unity.Common
{
    public interface IInAppPurchaseBase
    {
        string getPrice();
        string getCurrency();
        string getAdditionalParameters();
    }

    public interface IInAppPurchaseBaseBuilder
    {
        void withPrice(string price);
        void withCurrency(string currency);
        void withAdditionalParameters(Dictionary<string, string> additionalParameters);
    }

    public interface IPlayStoreInAppPurchase : IInAppPurchaseBase
    {
        Appodeal.PlayStorePurchaseType getPurchaseType();
        string getSku();
        string getOrderId();
        string getSignature();
        string getPublicKey();
        string getPurchaseData();
        string getPurchaseToken();
        string getDeveloperPayload();
        long getPurchaseTimestamp();
        IPlayStoreInAppPurchase NativeInAppPurchase { get; }
    }

    public interface IPlayStoreInAppPurchaseBuilder : IInAppPurchaseBaseBuilder
    {
        void withSku(string sku);
        void withOrderId(string orderId);
        void withPublicKey(string publicKey);
        void withSignature(string signature);
        void withPurchaseData(string purchaseData);
        void withPurchaseToken(string purchaseToken);
        void withDeveloperPayload(string developerPayload);
        void withPurchaseTimestamp(long purchaseTimestamp);
        IPlayStoreInAppPurchase build();
    }

    public interface IAppStoreInAppPurchase : IInAppPurchaseBase
    {
        Appodeal.AppStorePurchaseType getPurchaseType();
        string getProductId();
        string getTransactionId();
        IAppStoreInAppPurchase NativeInAppPurchase { get; }
    }

    public interface IAppStoreInAppPurchaseBuilder : IInAppPurchaseBaseBuilder
    {
        void withProductId(string productId);
        void withTransactionId(string transactionId);
        IAppStoreInAppPurchase build();
    }
}
