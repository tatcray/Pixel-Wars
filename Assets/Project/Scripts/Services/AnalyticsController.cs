using System.Collections.Generic;
using System.Text;
using Dependencies;
using Saves;
using UnityEngine;

namespace Services
{
    public static class AnalyticsController
    {
        private static int currentLvl => DataSaveLoader.SerializableData.wallIndex.Value;

        public static void SendLevelFailEvent()
        {
            SendAnalytics("LevelFail", GetDoubleParams("Level", currentLvl.ToString()));
        }

        public static void SendSessionStartPlay(int timeIndex)
        {
            SendAnalytics("SessionStart", GetDoubleParams("SessionStartTime", timeIndex.ToString()));
        }

        public static void SendPlayedTime(int timeIndex)
        {
            SendAnalytics("PlayTime", GetDoubleParams("PlayedTime", timeIndex.ToString()));
        }
        
        public static void SendPlayedSessionTime(int timeIndex)
        {
            SendAnalytics("SessionPlayTime", GetDoubleParams("SessionPlayedTime", timeIndex.ToString()));
        }
        
        public static void SendLevelCompletedEvent()
        {
            SendAnalytics("LevelComplete", GetDoubleParams("Level", currentLvl.ToString()));
        }
        
        public static void SendWeaponUpgradeEvent(string weapon, string upgrade, int lvl)
        {
            var parameters = new Dictionary<string, object> {
                {weapon.ToUpper(), $"{upgrade.ToUpper()}_{lvl}"},
                {"Level", currentLvl.ToString()}
            };
            
            SendAnalytics("Weapon_Upgrade", parameters);
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
            Debug.Log($"Send Event {eventName} {GetLogableParams(parameters)}");
            AppMetrica.Instance.ReportEvent(eventName, parameters);
        }

        private static string GetLogableParams(Dictionary<string, object> parameters)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var param in parameters)
            {
                stringBuilder.Append($" ({param.Key} {param.Value}) ");
            }

            return stringBuilder.ToString();
        }
    }
}