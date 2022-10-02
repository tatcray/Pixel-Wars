using System;
using Extensions;
using UnityEngine;

namespace Weapon
{
    public class AmmoTracker
    {
        public event Action AmmoEnded;

        private bool isTracking;
        
        public AmmoTracker(ObservableSerializedObject<int> ammo)
        {
            ammo.DataChanged += IsAmmoEnded;
        }

        public void StopTracking()
        {
            isTracking = false;
        }

        public void StartTracking()
        {
            isTracking = true;
        }

        private void IsAmmoEnded(int newAmmo)
        {
            if (isTracking && newAmmo <= 0)
            {
                AmmoEnded?.Invoke();
            }
        }
    }
}