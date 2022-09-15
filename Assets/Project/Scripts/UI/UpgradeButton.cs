using Dependencies;
using Extensions;
using Saves;
using Upgrades;

namespace UI
{
    public class UpgradeButton
    {
        private UpgradeButtonDependencies upgradeButtonDependencies;
        private UpgradeSystem upgradeSystem;
        private ObservableSerializedObject<int> money;
        public UpgradeButton(UpgradeButtonDependencies dependencies, UpgradeSystem upgradeSystem)
        {
            this.upgradeSystem = upgradeSystem;
            this.upgradeButtonDependencies = dependencies;
            money = DataSaveLoader.SerializableData.money;
            
            dependencies.
        }

        private void Upgrade()
        {
            
        }
    }
}