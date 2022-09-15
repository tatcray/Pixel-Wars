using System.Collections.Generic;
using Core;
using Dependencies;
using Extensions;
using Saves;
using UnityEngine.UI;
using Upgrades;

namespace UI
{
    public class GameScreen
    {
        private UIDependencies dependencies;
        private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
        
        public GameScreen(UIDependencies dependencies, UpgradeSystem upgradeSystem)
        {
            this.dependencies = dependencies;

            dependencies.playButton.onClick.AddListener(StartPlayMode);
            
            foreach (var button in dependencies.upgradeButtons)
                upgradeButtons.Add(new UpgradeButton(button, upgradeSystem));

            GameEvents.GameEndedByLose.Event += ShowMenuButtons;
            GameEvents.GameEndedByWin.Event += ShowMenuButtons;
        }

        public void SetMoney(int money)
        {
            dependencies.money.text = GetAdaptedValue(money);
        }

        public void SetAmmo(int ammo)
        {
            dependencies.ammo.text = ammo.ToString();
        }

        private void ShowCrosshair()
        {
            dependencies.crosshairCanvas.gameObject.SetActive(true);
        }

        private void HideCrosshair()
        {
            dependencies.crosshairCanvas.gameObject.SetActive(false);
        }

        private string GetAdaptedValue(int value)
        {
            if (value > 100000)
            {
                int thousands = value / 1000;
                int hundreds = value / 100;
                return $"{thousands}.{hundreds}k";
            }

            return value.ToString();
        }
        
        private void HideMenuButtons()
        {
            foreach (var button in dependencies.upgradeButtons)
                button.button.gameObject.SetActive(false);
            
            dependencies.playButton.gameObject.SetActive(false);
        }
        
        private void ShowMenuButtons()
        {
            foreach (var button in dependencies.upgradeButtons)
                button.button.gameObject.SetActive(true);
            
            dependencies.playButton.gameObject.SetActive(true);
        }

        private void StartPlayMode()
        {
            HideMenuButtons();
            ShowCrosshair();
        }

        private void StartMenuMode()
        {
            ShowMenuButtons();
            HideCrosshair();
        }
    }
}