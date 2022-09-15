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

            GameEvents.GameStarted.Event += ResetWeapon;

            new GlobalWeaponCubeHitParticles(references.hitEffectPrefab);
        }
        
        public void LoadWeapon(WeaponType type, WeaponConfig config)
        {
            Weapon newWeapon = cachedWeapons[type];
            
            if (currentWeapon != null && currentWeapon != newWeapon)
                currentWeapon.Disable();
            
            newWeapon.Activate();
            newWeapon.SetConfig(config);

            currentWeapon = newWeapon;
            defaultAmmo = config.ammo;
        }

        private void WeaponStartShoot()
        {
            currentWeapon.StartShoot();
        }

        private void WeaponStopShoot()
        {
            currentWeapon.StopShoot();
        }

        private void ResetWeapon()
        {
            currentWeapon?.Reset();
            ammo.Value = defaultAmmo;
        }

        private void AddWeaponToDictionary(WeaponType type)
        {
            WeaponDependency weaponDependency = weaponDependencies[type];
            
            cachedWeapons.Add(type, new Weapon(weaponDependency));
        }
    }
}