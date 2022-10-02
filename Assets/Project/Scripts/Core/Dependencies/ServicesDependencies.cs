using System;
using UnityEngine.Serialization;

namespace Dependencies
{
    [Serializable]
    public class ServicesDependencies
    {
        [FormerlySerializedAs("secondsTimeInterval")]
        public float analyticsSecondsTimeInterval;

        public string sdkKey;
        public string bannerId;
        public string rewardId;
        public string interstitialId;
    }
}