using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Common;

namespace AppodealAds.Unity.Android
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealInitializationCallback
#if UNITY_ANDROID
        : AndroidJavaProxy
    {
        private readonly IAppodealInitializationListener _listener;

        internal AppodealInitializationCallback(IAppodealInitializationListener listener) : base("com.appodeal.ads.initializing.ApdInitializationCallback")
        {
            _listener = listener;
        }

        private void onInitializationFinished(AndroidJavaObject errors)
        {
            if (errors == null)
            {
                _listener?.onInitializationFinished(null);
                return;
            }

            var errorsList = new List<string>();

            int countOfErrors = errors.Call<int>("size");
            for (int i = 0; i < countOfErrors; i++)
            {
                errorsList.Add(errors.Call<AndroidJavaObject>("get", i).Call<string>("toString"));
            }

            _listener?.onInitializationFinished(errorsList);
        }
    }
#else
    {
        public AppodealInitializationCallback(IAppodealInitializationListener listener) { }
    }
#endif
}
