using UnityEngine;
using System.Collections.Generic;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Dummy
{
    public class DummyPlayStoreInAppPurchaseBuilder : IPlayStoreInAppPurchaseBuilder
    {
        private const string DummyMessage = " To test in-app purchases, install your application on the Android/iOS device.";

        public void withPrice(string price)
        {
            Debug.Log("Call to withPrice on not supported platform." + DummyMessage);
        }

        public void withCurrency(string currency)
        {
            Debug.Log("Call to withCurrency on not supported platform." + DummyMessage);
        }

        public void withAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            Debug.Log("Call to withAdditionalParameters on not supported platform." + DummyMessage);
        }

        public void withPublicKey(string publicKey)
        {
            Debug.Log("Call to withPublicKey on not supported platform." + DummyMessage);
        }

        public void withSignature(string signature)
        {
            Debug.Log("Call to withSignature on not supported platform." + DummyMessage);
        }

        public void withPurchaseData(string purchaseData)
        {
            Debug.Log("Call to withPurchaseData on not supported platform." + DummyMessage);
        }

        public void withSku(string sku)
        {
            Debug.Log("Call to withSku on not supported platform." + DummyMessage);
        }

        public void withOrderId(string orderId)
        {
            Debug.Log("Call to withOrderId on not supported platform." + DummyMessage);
        }

        public void withPurchaseToken(string purchaseToken)
        {
            Debug.Log("Call to withPurchaseToken on not supported platform." + DummyMessage);
        }

        public void withPurchaseTimestamp(long purchaseTimestamp)
        {
            Debug.Log("Call to withPurchaseTimestamp on not supported platform." + DummyMessage);
        }

        public void withDeveloperPayload(string developerPayload)
        {
            Debug.Log("Call to withDeveloperPayload on not supported platform." + DummyMessage);
        }

        public IPlayStoreInAppPurchase build()
        {
            Debug.Log("Call to build on not supported platform." + DummyMessage);
            return null;
        }
    }
}
