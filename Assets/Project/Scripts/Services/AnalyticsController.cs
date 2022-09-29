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
            SendAnalytics("LevelFail", GetSingleParams($"Level_{currentLvl}"));
        }

        public static void SendSessionStartPlay(int timeIndex)
        {
            SendAnalytics("SessionStart", GetSingleParams($"SessionStartTime_{timeIndex}"));
        }

        public static void SendPlayedTime(int timeIndex)
        {
            SendAnalytics("PlayTime", GetSingleParams($"PlayedTime_{timeIndex}"));
        }
        
        public static void SendPlayedSessionTime(int timeIndex)
        {
            SendAnalytics("SessionPlayTime", GetSingleParams($"SessionPlayedTime_{timeIndex}"));
        }
        
        public static void SendLevelCompletedEvent()
        {
            SendAnalytics("LevelComplete", GetSingleParams($"Level_{currentLvl}"));
        }
        
        public static void SendTutorialPartComplete(int tutorialPart)
        {
            SendAnalytics("TutorialComplete", GetSingleParams($"TutorialPart_{tutorialPart}"));
        }
        
        public static void SendWeaponUpgradeEvent(string weapon, string upgrade, int lvl)
        {
            
            var parametersLevel2 = new Dictionary<string, object> {
                {$"Level_{currentLvl}", $"{upgrade}_{lvl}"}
            };
            
            var parameters = new Dictionary<string, object> {
                {weapon, parametersLevel2}
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
        #if UNITY_EDITOR
            Debug.Log($"Send Event {eventName} {GetLogableParams(parameters)}");
        #else
            AppMetrica.Instance.ReportEvent(eventName, parameters);
        #endif
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