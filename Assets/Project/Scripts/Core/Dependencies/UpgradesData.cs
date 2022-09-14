using System;
using System.Collections.Generic;
using Weapon;

namespace Dependencies
{
    [Serializable]
    public class UpgradesData
    {
        public List<WeaponUpgradesData> upgradesData;
    }
    
    [Serializable]
    public class WeaponUpgradesData
    {
        public WeaponType weaponType;
        public int weaponUpgradeCost;
        public List<Upgrade<int>> ammoUpgrades;
        public List<Upgrade<float>> radiusUpgrade;
        public List<Upgrade<float>> fireRateUpgrade;
    }

    [Serializable]
    public class Upgrade<T>
    {
        public T value;
        public int cost;
    }
}