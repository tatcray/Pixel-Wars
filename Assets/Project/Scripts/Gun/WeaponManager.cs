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
        private Dictionary<WeaponType, Weapon> cachedWeapons = new Dictionary<WeaponType, Weapon>();
        private Dictionary<WeaponType, WeaponDependency> weaponDependencies;

        private GameObject bulletPrefab;
        private Weapon currentWeapon;
        private GlobalWeaponCubeHitParticles currentHitParticles;
        
        public WeaponManager(WeaponReferences references, Crosshair crosshair)
        {
            weaponDependencies = references.weapons;
            
            AddWeaponToDictionary(WeaponType.Glock);
            AddWeaponToDictionary(WeaponType.Deagle);
            AddWeaponToDictionary(WeaponType.Nova);
            AddWeaponToDictionary(WeaponType.Mp7);
            AddWeaponToDictionary(WeaponType.P90);
            
            crosshair.ShootStarted += () => currentWeapon.StartShoot();
            crosshair.ShootStopped += () => currentWeapon.StopShoot();
            crosshair.PositionUpdated += newPosition => currentWeapon.LookAt(newPosition);

            GameEvents.GameEndedByLose.Event += () => currentWeapon?.Reset();
            GameEvents.GameEndedByWin.Event += () => currentWeapon?.Reset();

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
        }

        private void AddWeaponToDictionary(WeaponType type)
        {
            WeaponDependency weaponDependency = weaponDependencies[type];
            
            cachedWeapons.Add(type, new Weapon(weaponDependency));
        }
    }
}