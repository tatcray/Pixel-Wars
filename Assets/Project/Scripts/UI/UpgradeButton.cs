using Dependencies;
using Extensions;
using Saves;
using UnityEditor.PackageManager;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class UpgradeButton
    {
        private UpgradeButtonDependencies upgradeButtonDependencies;
        private UpgradeSystem upgradeSystem;
        private ObservableSerializedObject<int> money;

        private bool isEnabled;
        private int cost;
        private UpgradeType upgradeType;

        public UpgradeButton(UpgradeButtonDependencies dependencies, UpgradeSystem upgradeSystem)
        {
            this.upgradeSystem = upgradeSystem;
            this.upgradeButtonDependencies = dependencies;
            money = DataSaveLoader.SerializableData.money;
            this.upgradeType = dependencies.upgradeType;
            dependencies.button.onClick.AddListener(Upgrade);
            
            
        }
        
        public void Hide()
        {
            upgradeButtonDependencies.upgradeBackground.gameObject.SetActive(false);
        }
        public void Show()
        {
            upgradeButtonDependencies.upgradeBackground.gameObject.SetActive(true);
            CalculateState();
        }
        public void SetIcon(Sprite icon)
        {
            upgradeButtonDependencies.icon.sprite = icon;
        }
        
        private void SetCost(int money)
        {
            upgradeButtonDependencies.cost.text = money.ToString();
        }

        private void MakeUnavailable()
        {
            upgradeButtonDependencies.upgradeBackground.sprite = upgradeButtonDependencies.disableUpgradeBackground;
        }

        private void MakeAvailable()
        {
            upgradeButtonDependencies.upgradeBackground.sprite = upgradeButtonDependencies.activeUpgradeBackground;
        }


        private void CalculateState()
        {
            if (money.Value > cost && upgradeSystem.CanBeUpgraded(upgradeType))
            {
                MakeAvailable();
            }
            else
                MakeUnavailable();
        }
        
        private void Upgrade()
        {
            
        }

        private void SetNewCost()
        {
            int cost = upgradeSystem.GetCost(upgradeType);
            SetCost(cost);
        }
    }
}