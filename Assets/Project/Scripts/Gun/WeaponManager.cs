using System;
using System.Collections.Generic;
using Controller;
using Core;
using Dependencies;
using Extensions;
using UnityEngine;

namespace Weapon
{
    public class WeaponManager
    {
        public ObservableSerializedObject<int> ammo { get; } = new ObservableSerializedObject<int>();
        public ObservableSerializedObject<int> playingBullets { get; } = new ObservableSerializedObject<int>();

        private Dictionary<WeaponType, Weapon> cachedWeapons = new Dictionary<WeaponType, Weapon>();
        private Dictionary<WeaponType, WeaponDependency> weaponDependencies;

        private GameObject bulletPrefab;
        private Weapon currentWeapon;
        private int defaultAmmo;
        private GlobalWeaponCubeHitParticles currentHitParticles;
        
        public WeaponManager(WeaponReferences references, Crosshair crosshair)
        {
            weaponDependencies = references.weapons;
            
            AddWeaponToDictionary(WeaponType.Glock);
            AddWeaponToDictionary(WeaponType.Deagle);
            AddWeaponToDictionary(WeaponType.Nova);
            AddWeaponToDictionary(WeaponType.Mp7);
            AddWeaponToDictionary(WeaponType.P90);

            crosshair.ShootStarted += WeaponStartShoot;
            crosshair.ShootStopped += WeaponStopShoot;
            crosshair.PositionUpdated += newPosition => currentWeapon.LookAt(newPosition);

            GameEvents.GameEndedByLose.Event += ResetWeapon;
            GameEvents.GameEndedByWin.Event += ResetWeapon;
            GameEvents.GameStarted.Event += ResetWeapon;
            GameEvents.EndScreenShowed.Event += WeaponStopShoot;
            
            new GlobalWeaponCubeHitParticles(references.hitEffectPrefab);
        }
        
        public void LoadWeapon(WeaponType type, WeaponConfig config)
        {
            Weapon newWeapon = cachedWeapons[type];

            if (currentWeapon != null && currentWeapon != newWeapon)
            {
                currentWeapon.Shooted -= RegisterAmmoConsumption;
                newWeapon.flyingBullets.DataChanged -= RegisterFlyingBulletsChange;
                currentWeapon.Disable();
            }

            if (currentWeapon != newWeapon)
            {
                newWeapon.flyingBullets.DataChanged += RegisterFlyingBulletsChange;
                newWeapon.Shooted += RegisterAmmoConsumption;
            }

            newWeapon.Activate();
            newWeapon.SetConfig(config);

            currentWeapon = newWeapon;
            defaultAmmo = config.ammo;
            ammo.Value = defaultAmmo;
        }

        private void WeaponStartShoot()
        {
            if (ammo.Value > 0)
                currentWeapon.StartShoot();
        }

        private void WeaponStopShoot()
        {
            currentWeapon.StopShoot();
        }

        private void ResetWeapon()
        {
            ammo.Value = defaultAmmo;
            currentWeapon.Reset();
            currentWeapon.StopShoot();
        }

        private void RegisterAmmoConsumption()
        {
            ammo.Value--;
            
            if (ammo.Value <= 0)
                currentWeapon.StopShoot();
        }

        private void RegisterFlyingBulletsChange(int newFlyingBulletsCount)
        {
            playingBullets.Value = ammo.Value + newFlyingBulletsCount;
        }

        private void AddWeaponToDictionary(WeaponType type)
        {
            WeaponDependency weaponDependency = weaponDependencies[type];
            
            cachedWeapons.Add(type, new Weapon(weaponDependency));
        }
    }
}