using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Extensions;
using UnityEngine;

namespace Saves
{
    public class DataSaveLoader
    {
        private static readonly string fileName = "savedata";
        public static DataSave data { get; private set; } = new DataSave();

        public DataSaveLoader()
        {
            UnityEvents.ApplicationPause += Save;
            UnityEvents.ApplicationQuit += Save;
        }
        
        public DataSave LoadData()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(GetCombinedPath()))
            {
                Stream stream = new FileStream(GetCombinedPath(), FileMode.Open, FileAccess.ReadWrite);
                data = (DataSave)formatter.Deserialize(stream);
            }
            else
            {
                data = new DataSave();
                Save();
            }

            return data;
        }

        private void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(GetCombinedPath(), FileMode.Create, FileAccess.ReadWrite);

            formatter.Serialize(stream, data.GetSerializableSave());
            stream.Close();
        }

        private string GetCombinedPath()
        {
#if !UNITY_EDITOR
            string path = Path.Combine(Application.persistentDataPath, fileName);
#else
            string path = Path.Combine(Application.dataPath, fileName);
#endif
            return path;
        }
    }
}