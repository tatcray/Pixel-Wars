using UnityEngine;
using System.Collections.Generic;
using ConsentManager.Common;

namespace ConsentManager.Platforms.Dummy
{
    public class Dummy : IConsentManager, IConsentForm, IConsent, IVendor, IVendorBuilder, IConsentManagerException
    {
        private const string DummyMessage = "Not supported on this platform";

        public void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            Debug.Log(DummyMessage);
        }

        public void setCustomVendor(Vendor customVendor)
        {
            Debug.Log(DummyMessage);
        }

        public Vendor getCustomVendor(string bundle)
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public ConsentManager.Storage getStorage()
        {
            Debug.Log(DummyMessage);
            return ConsentManager.Storage.NONE;
        }

        public void setStorage(ConsentManager.Storage iabStorage)
        {
            Debug.Log(DummyMessage);
        }

        public Consent.ShouldShow shouldShowConsentDialog()
        {
            Debug.Log(DummyMessage);
            return Consent.ShouldShow.UNKNOWN;
        }

        public Consent.Zone getConsentZone()
        {
            Debug.Log(DummyMessage);
            return Consent.Zone.UNKNOWN;
        }

        public Consent.Status getConsentStatus()
        {
            Debug.Log(DummyMessage);
            return Consent.Status.UNKNOWN;
        }

        public Consent getConsent()
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public void disableAppTrackingTransparencyRequest()
        {
            Debug.Log(DummyMessage);
        }

        public void load()
        {
            Debug.Log(DummyMessage);
        }

        public void show()
        {
            Debug.Log(DummyMessage);
        }

        public bool isLoaded()
        {
            Debug.Log(DummyMessage);
            return false;
        }

        public bool isShowing()
        {
            Debug.Log(DummyMessage);
            return false;
        }

        public string getName()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public string getBundle()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public string getPolicyUrl()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public List<int> getPurposeIds()
        {
            Debug.Log(DummyMessage);
            return new List<int>();
        }

        public List<int> getFeatureIds()
        {
            Debug.Log(DummyMessage);
            return new List<int>();
        }

        public List<int> getLegitimateInterestPurposeIds()
        {
            Debug.Log(DummyMessage);
            return new List<int>();
        }

        IVendor IVendorBuilder.build()
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public void setPurposeIds(IEnumerable<int> purposeIds)
        {
            Debug.Log(DummyMessage);
        }

        public void setFeatureIds(IEnumerable<int> featureIds)
        {
            Debug.Log(DummyMessage);
        }

        public void setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            Debug.Log(DummyMessage);
        }

        public string getReason()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public int getCode()
        {
            Debug.Log(DummyMessage);
            return 0;
        }

        public Consent.Zone getZone()
        {
            Debug.Log(DummyMessage);
            return Consent.Zone.UNKNOWN;
        }

        public Consent.Status getStatus()
        {
            Debug.Log(DummyMessage);
            return Consent.Status.UNKNOWN;
        }

        public Consent.AuthorizationStatus getAuthorizationStatus()
        {
            Debug.Log(DummyMessage);
            return Consent.AuthorizationStatus.NOT_DETERMINED;
        }

        public Consent.HasConsent hasConsentForVendor(string bundle)
        {
            Debug.Log(DummyMessage);
            return Consent.HasConsent.UNKNOWN;
        }

        public string getIabConsentString()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }
    }
}
