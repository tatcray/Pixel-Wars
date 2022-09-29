using System;
using Dependencies;
using Extensions;
using UnityEngine.UI;

namespace Tutorial
{
    public class UpgradeTutorial : ButtonTutorial
    {
        private TutorialDependencies dependencies;
        public UpgradeTutorial(TutorialDependencies dependencies, ObservableSerializedObject<int> money) : base(dependencies, dependencies.upgradeButton)
        {
            this.dependencies = dependencies;
            if (money.Value < dependencies.minimalMoneyOnUpgradePart)
                money.Value = dependencies.minimalMoneyOnUpgradePart;
        }

        public override void StartTutorial(Action completedCallback)
        {
            base.StartTutorial(completedCallback);
            
            foreach (Button button in dependencies.buttonsToDisableOnUpgradePart)
                button.enabled = false;
        }


        protected override void CompleteTutorial()
        {
            base.CompleteTutorial();
            
            foreach (Button button in dependencies.buttonsToDisableOnUpgradePart)
                button.enabled = true;
        }
    }
}