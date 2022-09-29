using System;
using Extensions;
using UnityEngine;

namespace Weapon
{
    public class AmmoTracker
    {
        public event Action AmmoEnded;
        
        public AmmoTracker(ObservableSerializedObject<int> ammo)
        {
            ammo.DataChanged += IsAmmoEnded;
        }

        private void IsAmmoEnded(int newAmmo)
        {
            if (newAmmo <= 0)
            {
                AmmoEnded?.Invoke();
            }
        }
    }
}