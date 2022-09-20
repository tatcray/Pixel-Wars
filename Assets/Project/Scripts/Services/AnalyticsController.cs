using System.Collections.Generic;
using Dependencies;
using Saves;

namespace Services
{
    public static class AnalyticsController
    {
        private static int currentLvl => DataSaveLoader.SerializableData.wallIndex.Value;

        public static void SendLevelFailEvent()
        {
            SendAnalytics("LevelFail", GetSingleParams($"Level_{currentLvl}"));
        }
        
        public static void SendLevelCompletedEvent()
        {
            SendAnalytics("LevelComplete", GetSingleParams($"Level_{currentLvl}"));
        }
        
        public static void SendWeaponUpgradeEvent(string weapon, string upgrade)
        {
            
        }
        
        public static void SendAdViewEvent(string adName)
        {
            Dictionary<string, object> eventParams = new Dictionary<string, object>
            {
                {adName, $"Level_{currentLvl}"}
            };
            SendAnalytics(adName, eventParams);
        }

        private static Dictionary<string, object> GetSingleParams(string param)
        {
            return new Dictionary<string, object>{ {param, param} };
        }
        
        private static Dictionary<string, object> GetDoubleParams(string first, string second)
        {
            return new Dictionary<string, object>{ {first, second} };
        }

        private static void SendAnalytics(string eventName, Dictionary<string, object> parameters)
        {
            AppMetrica.Instance.ReportEvent(eventName, parameters);
        }
    }
}