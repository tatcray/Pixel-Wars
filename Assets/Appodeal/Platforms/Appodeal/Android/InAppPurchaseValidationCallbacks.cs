using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Android
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class InAppPurchaseValidationCallbacks
#if UNITY_ANDROID
        : AndroidJavaProxy
    {
        private readonly IInAppPurchaseValidationListener _listener;

        internal InAppPurchaseValidationCallbacks(IInAppPurchaseValidationListener listener) : base("com.appodeal.ads.inapp.InAppPurchaseValidateCallback")
        {
            _listener = listener;
        }

        private void onInAppPurchaseValidateSuccess(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            _listener?.onInAppPurchaseValidationSucceeded(CreateResponse(purchase, errors));
        }

        private void onInAppPurchaseValidateFail(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            _listener?.onInAppPurchaseValidationFailed(CreateResponse(purchase, errors));
        }

        private string CreateResponse(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            var androidPurchase = new AndroidPlayStoreInAppPurchase(purchase);

            string responsePurchase = "\"InAppPurchase\":{";
            responsePurchase += $"\"PublicKey\":\"{androidPurchase.getPublicKey() ?? String.Empty}\",";
            responsePurchase += $"\"Signature\":\"{androidPurchase.getSignature() ?? String.Empty}\",";
            responsePurchase += $"\"PurchaseData\":\"{androidPurchase.getPurchaseData() ?? String.Empty}\",";
            responsePurchase += $"\"Price\":\"{androidPurchase.getPrice() ?? String.Empty}\",";
            responsePurchase += $"\"Currency\":\"{androidPurchase.getCurrency() ?? String.Empty}\",";
            responsePurchase += $"\"Sku\":\"{androidPurchase.getSku() ?? String.Empty}\",";
            responsePurchase += $"\"OrderId\":\"{androidPurchase.getOrderId() ?? String.Empty}\",";
            responsePurchase += $"\"PurchaseToken\":\"{androidPurchase.getPurchaseToken() ?? String.Empty}\",";
            responsePurchase += $"\"PurchaseTimestamp\":\"{androidPurchase.getPurchaseTimestamp()}\",";
            responsePurchase += $"\"AdditionalParameters\":\"{androidPurchase.getAdditionalParameters() ?? String.Empty}\",";
            responsePurchase += $"\"DeveloperPayload\":\"{androidPurchase.getDeveloperPayload() ?? String.Empty}\"}}";

            string responseError = "\"Errors\":[";
            if (errors != null)
            {
                var errorsList = new List<string>();
                int countOfErrors = errors.Call<int>("size");
                for (int i = 0; i < countOfErrors; i++)
                {
                    errorsList.Add($"\"{errors.Call<AndroidJavaObject>("get", i).Call<string>("toString")}\"");
                }
                responseError += String.Join(",", errorsList);
            }
            responseError += ']';

            return $"{{{responsePurchase},{responseError}}}";
        }
    }
#else
    {
        public InAppPurchaseValidationCallbacks(IInAppPurchaseValidationListener listener) { }
    }
#endif
}
