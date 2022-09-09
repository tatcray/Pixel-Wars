using System;
using UnityEngine;

namespace Extensions
{
    public static class UnityEvents
    {
        private static readonly string gameObjectName = "CoroutineHolder";
        private static UnityEventsListener eventsListener;
        
        public static event Action Update;
        public static event Action ApplicationPause;
        public static event Action ApplicationUnPause;
        public static event Action ApplicationQuit;

        static UnityEvents()
        {
            eventsListener = new GameObject(gameObjectName).AddComponent<UnityEventsListener>();
            eventsListener.UpdateInvoked += () => Update?.Invoke();
            eventsListener.ApplicationPaused += () => ApplicationPause?.Invoke();
            eventsListener.ApplicationUnPaused += () => ApplicationUnPause?.Invoke();
            eventsListener.ApplicationQuited += () => ApplicationQuit?.Invoke();
        }

        private class UnityEventsListener : MonoBehaviour
        {
            public event Action UpdateInvoked;
            public event Action ApplicationPaused;
            public event Action ApplicationUnPaused;
            public event Action ApplicationQuited;
            
            private void Update() => UpdateInvoked?.Invoke();

            private void OnApplicationQuit() => ApplicationQuited?.Invoke();

            private void OnApplicationPause(bool pauseStatus)
            {
                if (pauseStatus)
                    ApplicationPaused?.Invoke();
                else
                    ApplicationUnPaused?.Invoke();
            }
        }
    }
}