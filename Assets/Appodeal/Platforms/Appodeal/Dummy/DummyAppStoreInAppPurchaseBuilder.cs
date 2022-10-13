using UnityEngine;
using System.Collections.Generic;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Dummy
{
    public class DummyAppStoreInAppPurchaseBuilder : IAppStoreInAppPurchaseBuilder
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

        public void withProductId(string productId)
        {
            Debug.Log("Call to withProductId on not supported platform." + DummyMessage);
        }

        public void withTransactionId(string transactionId)
        {
            Debug.Log("Call to withTransactionId on not supported platform." + DummyMessage);
        }

        public IAppStoreInAppPurchase build()
        {
            Debug.Log("Call to build on not supported platform." + DummyMessage);
            return null;
        }
    }
}
