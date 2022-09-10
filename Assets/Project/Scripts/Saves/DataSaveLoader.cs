using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Extensions;
using UnityEngine;

namespace Saves
{
    public class DataSaveLoader
    {
        private static readonly string fileName = "savedata";
        public static BinaryDataSave BinaryData { get; private set; } = new BinaryDataSave();

        public DataSaveLoader()
        {
            UnityEvents.ApplicationPause += Save;
            UnityEvents.ApplicationQuit += Save;
        }
        
        public BinaryDataSave LoadData()
        {
            if (File.Exists(GetCombinedPath()))
            {
                Load();
            }
            else
            {
                BinaryData = new BinaryDataSave();
                Save();
            }

            return BinaryData;
        }

        private void Load()
        {
            Stream stream = new FileStream(GetCombinedPath(), FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(stream);
            
            BinaryData.Deserialize(reader);
            
            reader.Close();
            stream.Close();
        }

        private void Save()
        {
            Stream stream = new FileStream(GetCombinedPath(), FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter writer = new BinaryWriter(stream);

            BinaryData.Serialize(writer);
            
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