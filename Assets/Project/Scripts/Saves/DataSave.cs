using System;
using Extensions;
using Upgrades;

namespace Saves
{
    [Serializable]
    public class DataSave
    {
        public ObservableSerializedObject<int> wallIndex = new ObservableSerializedObject<int>();
        public ObservableSerializedObject<int> money = new ObservableSerializedObject<int>();
        public SerializableDictionary<UpgradeType, int> upgrades = new SerializableDictionary<UpgradeType, int>();

        public DataSave GetSerializableSave()
        {
            DataSave serializableSave = new DataSave();
            serializableSave.wallIndex.Value = wallIndex.Value;
            serializableSave.money.Value = money.Value;
            serializableSave.upgrades = new SerializableDictionary<UpgradeType, int>(upgrades);

            return serializableSave;
        }
    }
}