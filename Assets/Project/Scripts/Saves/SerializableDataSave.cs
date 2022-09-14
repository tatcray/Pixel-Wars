using System;
using System.IO;
using Extensions;
using Upgrades;

namespace Saves
{
    [Serializable]
    public class SerializableDataSave
    {
        public ObservableSerializedObject<int> wallIndex = new ObservableSerializedObject<int>();
        public ObservableSerializedObject<int> money = new ObservableSerializedObject<int>();
        public SerializableDictionary<UpgradeType, int> upgrades = new SerializableDictionary<UpgradeType, int>();

        public SerializableDataSave()
        {
            upgrades[UpgradeType.Weapon] = 0;
            upgrades[UpgradeType.Ammo] = 0;
            upgrades[UpgradeType.FireRate] = 0;
            upgrades[UpgradeType.Radius] = 0;
        }
        
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(wallIndex.Value);
            writer.Write(money.Value);
            
            writer.Write(upgrades.Count);
            foreach (var keyValue in upgrades)
            {
                writer.Write((int)keyValue.Key);
                writer.Write(keyValue.Value);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            wallIndex.Value = reader.ReadInt32();
            money.Value = reader.ReadInt32();

            upgrades.Clear();
            int upgradesLength = reader.ReadInt32();
            for (int i = 0; i < upgradesLength; i++)
            {
                UpgradeType type = (UpgradeType)reader.ReadInt32();
                int lvl = reader.ReadInt32();
                upgrades.Add(type, lvl);
            }
        }
    }
}