using System.IO;
using Extensions;
using UnityEngine;

namespace Saves
{
    public class DataSaveLoader
    {
        private static readonly string fileName = "savedata";
        public static SerializableDataSave SerializableData { get; private set; } = new SerializableDataSave();

        public DataSaveLoader()
        {
            UnityEvents.ApplicationPause += Save;
            UnityEvents.ApplicationQuit += Save;
        }
        
        public SerializableDataSave LoadData()
        {
            if (File.Exists(GetCombinedPath()))
            {
                Load();
            }
            else
            {
                SerializableData = new SerializableDataSave();
                Save();
            }

            return SerializableData;
        }

        private void Load()
        {
            Stream stream = new FileStream(GetCombinedPath(), FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(stream);
            
            SerializableData.Deserialize(reader);
            
            reader.Close();
            stream.Close();
        }

        private void Save()
        {
            Stream stream = new FileStream(GetCombinedPath(), FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter writer = new BinaryWriter(stream);

            SerializableData.Serialize(writer);
            
            writer.Close();
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