using System;
using System.Collections.Generic;
using Dependencies;
using Extensions;
using Weapon;

namespace Upgrades
{
    public class UpgradeSystem
    {
        private Dictionary<WeaponType, WeaponUpgradesData> weaponUpgradesDatas =
            new Dictionary<WeaponType, WeaponUpgradesData>();
        private SerializableDictionary<UpgradeType, int> upgrades = new SerializableDictionary<UpgradeType, int>();
        
        private WeaponManager weaponManager;
        private WeaponUpgradesData currentUpgradesData;
        public UpgradeSystem(UpgradesData upgradesData,  WeaponManager weaponManager, SerializableDictionary<UpgradeType, int> upgrades)
        {
            this.weaponManager = weaponManager;
            this.upgrades = upgrades;

            foreach (WeaponUpgradesData upgradeData in upgradesData.upgradesData)
                weaponUpgradesDatas.Add(upgradeData.weaponType, upgradeData);

            currentUpgradesData = weaponUpgradesDatas[GetCurrentWeaponType()];
            SendNewConfigToWeaponManager();
        }
        
        public void Upgrade(UpgradeType type)
        {
            if (CanBeUpgraded(type))
            {
                upgrades[type]++;

                if (type == UpgradeType.Weapon)
                    ResetAllAttachedWeaponUpgrades();
                
                SendNewConfigToWeaponManager();
            }
        }

        public int GetCost(UpgradeType type)
        {
            int lvl = upgrades[type];
            switch (type)
            {
                case UpgradeType.Ammo:
                    return currentUpgradesData.ammoUpgrades[lvl].cost;
                case UpgradeType.Radius:
                    return currentUpgradesData.radiusUpgrade[lvl].cost;
                case UpgradeType.FireRate:
                    return currentUpgradesData.fireRateUpgrade[lvl].cost;
                case UpgradeType.Weapon:
                    return currentUpgradesData.weaponUpgradeCost;
                
                default:
                    return default;
            }
        }

        public bool CanBeUpgraded(UpgradeType type)
        {
            int currentLvl = GetCurrentLvl(type);
            
            switch (type)
            {
                case UpgradeType.Ammo:
                    return currentLvl < currentUpgradesData.ammoUpgrades.Count - 1;
                case UpgradeType.Radius:
                    return currentLvl < currentUpgradesData.ammoUpgrades.Count - 1;
                case UpgradeType.FireRate:
                    return currentLvl < currentUpgradesData.ammoUpgrades.Count - 1;
                case UpgradeType.Weapon:
                    return currentLvl < Enum.GetNames(typeof(WeaponType)).Length - 1;
                
                default:
                    return default;
            }
        }
        
        private void SendNewConfigToWeaponManager()
        {
            WeaponConfig config = new WeaponConfig();

            config.ammo = currentUpgradesData.ammoUpgrades[upgrades[UpgradeType.Ammo]].value;
            config.fireRate = currentUpgradesData.fireRateUpgrade[upgrades[UpgradeType.FireRate]].value;
            config.radius = currentUpgradesData.radiusUpgrade[upgrades[UpgradeType.Radius]].value;

            weaponManager.LoadWeapon(GetCurrentWeaponType(), config);
        }

        private int GetCurrentLvl(UpgradeType upgradeType)
        {
            return upgrades[upgradeType];
        }

        private WeaponType GetCurrentWeaponType()
        {
            return (WeaponType)upgrades[UpgradeType.Weapon];
        }

        private void ResetAllAttachedWeaponUpgrades()
        {
            upgrades[UpgradeType.Radius] = 0;
            upgrades[UpgradeType.FireRate] = 0;
            upgrades[UpgradeType.Ammo] = 0;
        }
    }
}