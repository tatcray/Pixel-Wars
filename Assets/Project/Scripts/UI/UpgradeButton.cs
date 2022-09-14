using UnityEngine;
using Upgrades;

namespace UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField]
        private UpgradeType upgradeType;

        private UpgradeSystem upgradeSystem;

        public void Initialize(UpgradeSystem upgradeSystem)
        {
            this.upgradeSystem = upgradeSystem;
        }

        public void Upgrade()
        {
            upgradeSystem.Upgrade(upgradeType);
        }
    }
}