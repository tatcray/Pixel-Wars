using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ConsentManager.Common;

namespace ConsentManager.Platforms.Android
{
#if UNITY_ANDROID
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AndroidConsentManager : IConsentManager
    {
        private AndroidJavaObject consentManagerInstance;
        private AndroidJavaObject activity;

        private AndroidJavaObject getInstance()
        {
            return consentManagerInstance ?? (consentManagerInstance = new AndroidJavaObject("com.appodeal.consent.ConsentManager"));
        }

        private AndroidJavaObject getActivity()
        {
            return activity ?? (activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
        }

        public void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            getInstance().CallStatic("requestConsentInfoUpdate", getActivity(), appodealAppKey, new ConsentInfoUpdateCallbacks(listener));
        }

        public void setCustomVendor(Vendor customVendor)
        {
            var androidVendor = (AndroidVendor) customVendor.getNativeVendor();
            getInstance().CallStatic<AndroidJavaObject>("getCustomVendors").Call<AndroidJavaObject>("put", androidVendor.getBundle(), androidVendor.getVendor());
        }

        public Vendor getCustomVendor(string bundle)
        {
            return new Vendor(new AndroidVendor(getInstance().CallStatic<AndroidJavaObject>("getCustomVendors").Call<AndroidJavaObject>("get", Helper.getJavaObject(bundle))));
        }

        public ConsentManager.Storage getStorage()
        {
            var storage = ConsentManager.Storage.NONE;
            switch (getInstance().CallStatic<AndroidJavaObject>("getStorage").Call<string>("name"))
            {
                case "NONE":
                    storage = ConsentManager.Storage.NONE;
                    break;
                case "SHARED_PREFERENCE":
                    storage = ConsentManager.Storage.SHARED_PREFERENCE;
                    break;
            }

            return storage;
        }

        public void setStorage(ConsentManager.Storage iabStorage)
        {
            switch (iabStorage)
            {
                case ConsentManager.Storage.NONE:
                    getInstance().CallStatic("setStorage", new AndroidJavaClass("com.appodeal.consent.ConsentManager$Storage").GetStatic<AndroidJavaObject>("NONE"));
                    break;
                case ConsentManager.Storage.SHARED_PREFERENCE:
                    getInstance().CallStatic("setStorage", new AndroidJavaClass("com.appodeal.consent.ConsentManager$Storage").GetStatic<AndroidJavaObject>("SHARED_PREFERENCE"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(iabStorage), iabStorage, null);
            }
        }

        public Consent.ShouldShow shouldShowConsentDialog()
        {
            var shouldShow = Consent.ShouldShow.UNKNOWN;

            switch (getInstance().CallStatic<bool>("getShouldShow"))
            {
                case true:
                    shouldShow = Consent.ShouldShow.TRUE;
                    break;
                case false:
                    shouldShow = Consent.ShouldShow.FALSE;
                    break;
            }

            return shouldShow;
        }

        public Consent.Zone getConsentZone()
        {
            var zone = Consent.Zone.UNKNOWN;
            switch (getInstance().CallStatic<AndroidJavaObject>("getConsentZone").Call<string>("name"))
            {
                case "UNKNOWN":
                    zone = Consent.Zone.UNKNOWN;
                    break;
                case "NONE":
                    zone = Consent.Zone.NONE;
                    break;
                case "GDPR":
                    zone = Consent.Zone.GDPR;
                    break;
                case "CCPA":
                    zone = Consent.Zone.CCPA;
                    break;
            }

            return zone;
        }

        public Consent.Status getConsentStatus()
        {
            var status = Consent.Status.UNKNOWN;
            switch (getInstance().CallStatic<AndroidJavaObject>("getConsentStatus").Call<string>("name"))
            {
                case "UNKNOWN":
                    status = Consent.Status.UNKNOWN;
                    break;
                case "PERSONALIZED":
                    status = Consent.Status.PERSONALIZED;
                    break;
                case "NON_PERSONALIZED":
                    status = Consent.Status.NON_PERSONALIZED;
                    break;
                case "PARTLY_PERSONALIZED":
                    status = Consent.Status.PARTLY_PERSONALIZED;
                    break;
            }

            return status;
        }

        public Consent getConsent()
        {
            return new Consent(new AndroidConsent(getInstance().CallStatic<AndroidJavaObject>("getConsent")));
        }

        public void disableAppTrackingTransparencyRequest()
        {
            Debug.Log("Not supported on Android platform");
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AndroidVendorBuilder : IVendorBuilder
    {
        private readonly AndroidJavaObject builder;
        private AndroidJavaObject vendor;

        public AndroidVendorBuilder(string name, string bundle, string policyUrl)
        {
            builder = new AndroidJavaObject("com.appodeal.consent.Vendor$Builder", name, bundle, policyUrl);
        }

        private AndroidJavaObject getBuilder()
        {
            return builder;
        }

        public IVendor build()
        {
            vendor = getBuilder().Call<AndroidJavaObject>("build");
            return new AndroidVendor(vendor);
        }

        public void setPurposeIds(IEnumerable<int> purposeIds)
        {
            setNativeList(purposeIds, "purposeIds");
        }

        public void setFeatureIds(IEnumerable<int> featureIds)
        {
            setNativeList(featureIds, "featureIds");
        }

        public void setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            setNativeList(legitimateInterestPurposeIds, "legitimateInterestPurposeIds");
        }

        private void setNativeList(IEnumerable<int> list, string methodName)
        {
            var androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
            foreach (var obj in list)
            {
                androidJavaObject.Call<bool>("add", Helper.getJavaObject(obj));
            }

            getBuilder().Call<AndroidJavaObject>(methodName, androidJavaObject);
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AndroidVendor : IVendor
    {
        private readonly AndroidJavaObject vendor;

        public AndroidVendor(AndroidJavaObject vendor)
        {
            this.vendor = vendor;
        }

        public AndroidJavaObject getVendor()
        {
            return vendor;
        }

        public string getName()
        {
            return getVendor().Call<string>("getName");
        }

        public string getBundle()
        {
            return getVendor().Call<string>("getBundle");
        }

        public string getPolicyUrl()
        {
            return getVendor().Call<string>("getPolicyUrl");
        }

        public List<int> getPurposeIds()
        {
            return getNativeList("getPurposeIds", getVendor());
        }

        public List<int> getFeatureIds()
        {
            return getNativeList("getFeatureIds", getVendor());
        }

        public List<int> getLegitimateInterestPurposeIds()
        {
            return getNativeList("getLegitimateInterestPurposeIds", getVendor());
        }

        private static List<int> getNativeList(string methodName, AndroidJavaObject androidJavaObject)
        {
            var purposeIdsList = new List<int>();
            AndroidJNI.PushLocalFrame(100);
            using (var joPurposeIdsList = androidJavaObject.Call<AndroidJavaObject>(methodName))
            {
                for (var i = 0; i < joPurposeIdsList.Call<int>("size"); i++)
                {
                    using (var PurposeId = joPurposeIdsList.Call<AndroidJavaObject>("get", i))
                    {
                        purposeIdsList.Add(PurposeId.Call<int>("intValue"));
                    }
                }
            }

            AndroidJNI.PopLocalFrame(IntPtr.Zero);
            return purposeIdsList;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AndroidConsentForm : IConsentForm
    {
        private readonly AndroidJavaObject consentForm;
        
        private AndroidJavaObject activity;

        public AndroidConsentForm(AndroidJavaObject builder)
        {
            consentForm = builder;
        }

        public AndroidConsentForm(IConsentFormListener consentFormListener)
        {
            consentForm = new AndroidJavaObject("com.appodeal.consent.ConsentForm", getActivity(), new ConsentFormCallbacks(consentFormListener));
        }

        private AndroidJavaObject getConsentForm()
        {
            return consentForm;
        }

        private AndroidJavaObject getActivity()
        {
            return activity ?? (activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
        }

        public void load()
        {
            getConsentForm().Call("load");
        }

        public void show()
        {
            getConsentForm().Call("show");
        }

        public bool isLoaded()
        {
            return getConsentForm().Call<bool>("isLoaded");
        }

        public bool isShowing()
        {
            return getConsentForm().Call<bool>("isShowing");
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AndroidConsentManagerException : IConsentManagerException
    {
        private readonly AndroidJavaObject consentManagerException;

        public AndroidConsentManagerException(AndroidJavaObject androidJavaObject)
        {
            consentManagerException = androidJavaObject;
        }

        public AndroidConsentManagerException()
        {
            consentManagerException = new AndroidJavaObject("com.appodeal.consent.ConsentManagerError");
        }

        private AndroidJavaObject getConsentManagerException()
        {
            return consentManagerException;
        }

        public string getReason()
        {
            return getConsentManagerException().Call<string>("getMessage");
        }

        public int getCode()
        {
            string reason = getConsentManagerException().Call<string>("getEvent");
            return reason == "LoadingError" ? 2 : 1;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AndroidConsent : IConsent
    {
        private readonly AndroidJavaObject consent;

        public AndroidConsent(AndroidJavaObject joConsent)
        {
            consent = joConsent;
        }

        public AndroidJavaObject getConsent()
        {
            return consent;
        }

        public Consent.Zone getZone()
        {
            var zone = Consent.Zone.UNKNOWN;

            switch (getConsent().Call<AndroidJavaObject>("getZone").Call<string>("name"))
            {
                case "UNKNOWN":
                    zone = Consent.Zone.UNKNOWN;
                    break;
                case "NONE":
                    zone = Consent.Zone.NONE;
                    break;
                case "GDPR":
                    zone = Consent.Zone.GDPR;
                    break;
                case "CCPA":
                    zone = Consent.Zone.CCPA;
                    break;
            }

            return zone;
        }

        public Consent.Status getStatus()
        {
            var status = Consent.Status.UNKNOWN;

            switch (getConsent().Call<AndroidJavaObject>("getStatus").Call<string>("name"))
            {
                case "UNKNOWN":
                    status = Consent.Status.UNKNOWN;
                    break;
                case "NON_PERSONALIZED":
                    status = Consent.Status.NON_PERSONALIZED;
                    break;
                case "PARTLY_PERSONALIZED":
                    status = Consent.Status.PARTLY_PERSONALIZED;
                    break;
                case "PERSONALIZED":
                    status = Consent.Status.PERSONALIZED;
                    break;
            }

            return status;
        }

        public Consent.AuthorizationStatus getAuthorizationStatus()
        {
            Debug.Log("Not supported on this platform");
            return Consent.AuthorizationStatus.NOT_DETERMINED;
        }

        public Consent.HasConsent hasConsentForVendor(string bundle)
        {
            var hasConsent = Consent.HasConsent.UNKNOWN;
            switch (getConsent().Call<bool>("hasConsentForVendor", Helper.getJavaObject(bundle)))
            {
                case true:
                    hasConsent = Consent.HasConsent.TRUE;
                    break;
                case false:
                    hasConsent = Consent.HasConsent.FALSE;
                    break;
            }

            return hasConsent;
        }

        public List<Vendor> getAcceptedVendors()
        {
            var vendors = new List<Vendor>();
            AndroidJNI.PushLocalFrame(100);
            using (var joPurposeIdsList = getConsent().Call<AndroidJavaObject>("getAcceptedVendors"))
            {
                for (var i = 0; i < joPurposeIdsList.Call<int>("size"); i++)
                {
                    using (var vendor = joPurposeIdsList.Call<AndroidJavaObject>("get", i))
                    {
                        vendors.Add(new Vendor(new AndroidVendor(vendor)));
                    }
                }
            }

            AndroidJNI.PopLocalFrame(IntPtr.Zero);
            return vendors;
        }

        public string getIabConsentString()
        {
            return getConsent().Call<string>("getIABConsentString");
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition")]
    public static class Helper
    {
        public static object getJavaObject(object value)
        {
            if (value is string)
            {
                return value;
            }

            if (value is char)
            {
                return new AndroidJavaObject("java.lang.Character", value);
            }

            if ((value is bool))
            {
                return new AndroidJavaObject("java.lang.Boolean", value);
            }

            if (value is int)
            {
                return new AndroidJavaObject("java.lang.Integer", value);
            }

            if (value is long)
            {
                return new AndroidJavaObject("java.lang.Long", value);
            }

            if (value is float)
            {
                return new AndroidJavaObject("java.lang.Float", value);
            }

            if (value is double)
            {
                return new AndroidJavaObject("java.lang.Float", value);
            }

            return value ?? null;
        }
    }
#endif
}
