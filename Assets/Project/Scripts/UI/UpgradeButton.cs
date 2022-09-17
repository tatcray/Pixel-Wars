using Dependencies;
using Extensions;
using Saves;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class UpgradeButton
    {
        protected UpgradeButtonDependencies upgradeButtonDependencies;
        protected UpgradeSystem upgradeSystem;
        protected UpgradeType upgradeType;
        
        private ObservableSerializedObject<int> money;

        private bool isEnabled;
        private int cost;

        public UpgradeButton(UpgradeButtonDependencies dependencies, UpgradeSystem upgradeSystem)
        {
            this.upgradeSystem = upgradeSystem;
            this.upgradeButtonDependencies = dependencies;
            money = DataSaveLoader.SerializableData.money;
            this.upgradeType = dependencies.upgradeType;
            dependencies.button.onClick.AddListener(Upgrade);
            
            upgradeSystem.Upgraded += CalculateState;
            
            CalculateState();
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
        
        protected virtual void CalculateState()
        {
            SetNewCost();
            
            if (money.Value > cost && upgradeSystem.CanBeUpgraded(upgradeType))
            {
                MakeAvailable();
            }
            else
            {
                if (!upgradeSystem.CanBeUpgraded(upgradeType))
                {
                  upgradeButtonDependencies.costIcon.enabled = false;
                  upgradeButtonDependencies.cost.text = "";
                }
                MakeUnavailable();  
            }
        }
        
        private void SetCost(int money)
        {
            upgradeButtonDependencies.cost.text = UIExtensions.GetAdaptedValue(money, "<font=\"Characters\" material=\"CostsText\">");
        }

        private void MakeUnavailable()
        {
            upgradeButtonDependencies.upgradeBackground.sprite = upgradeButtonDependencies.disableUpgradeBackground;
            upgradeButtonDependencies.button.enabled = false;
            isEnabled = false;
        }

        private void MakeAvailable()
        {
            upgradeButtonDependencies.upgradeBackground.sprite = upgradeButtonDependencies.activeUpgradeBackground;
            upgradeButtonDependencies.button.enabled = true;
            upgradeButtonDependencies.costIcon.enabled = true;
            isEnabled = true;
        }

        private void Upgrade()
        {
            if (!isEnabled)
                return;
            
            SetNewCost();
            money.Value -= cost;
            
            upgradeSystem.Upgrade(upgradeType);
        }

        private void SetNewCost()
        {
            cost = upgradeSystem.GetCost(upgradeType);
            SetCost(cost);
        }
    }
}