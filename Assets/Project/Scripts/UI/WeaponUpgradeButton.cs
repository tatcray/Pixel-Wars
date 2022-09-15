using System.Collections.Generic;
using Dependencies;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class WeaponUpgradeButton : UpgradeButton
    {
        private List<Sprite> sprites;
        public WeaponUpgradeButton(List<Sprite> weaponSprites, UpgradeButtonDependencies dependencies, UpgradeSystem upgradeSystem) 
            : base(dependencies, upgradeSystem)
        {
            sprites = weaponSprites;
        }
        
        protected override void CalculateState()
        {
            if (sprites != null)
                upgradeButtonDependencies.icon.sprite = sprites[upgradeSystem.GetLvl(upgradeType)];
            
            base.CalculateState();
        }
    }
}