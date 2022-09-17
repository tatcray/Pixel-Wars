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
        private UpgradeSystem upgradeSystem;
        private List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();
        
        public GameScreen(UIDependencies dependencies, UpgradeSystem upgradeSystem)
        {
            this.dependencies = dependencies;
            this.upgradeSystem = upgradeSystem;

            dependencies.playButton.onClick.AddListener(StartPlayMode);

            CreateButtons();

            GameEvents.GameEndedByLose.Event += StartMenuMode;
            GameEvents.GameEndedByWin.Event += StartMenuMode;
            
            StartMenuMode();
        }

        public void SetMoney(int money)
        {
            dependencies.money.text = UIExtensions.GetAdaptedValue(money, "<font=\"Characters\" material=\"MoneyText\">");
        }

        public void SetAmmo(int ammo)
        {
            dependencies.ammo.text = ammo.ToString();
        }

        private void CreateButtons()
        {
            foreach (var button in dependencies.upgradeButtons)
            {
                if (button.upgradeType == UpgradeType.Weapon)
                    upgradeButtons.Add(new WeaponUpgradeButton(dependencies.weaponSprites, button, upgradeSystem));
                else
                    upgradeButtons.Add(new UpgradeButton(button, upgradeSystem));
            }
        }

        private void ShowCrosshair()
        {
            dependencies.crosshairCanvas.gameObject.SetActive(true);
        }

        private void HideCrosshair()
        {
            dependencies.crosshairCanvas.gameObject.SetActive(false);
        }
        
        private void HideMenuButtons()
        {
            foreach (var button in dependencies.upgradeButtons)
                button.button.gameObject.SetActive(false);

            foreach (var button in upgradeButtons)
                button.Hide();
            
            dependencies.playButton.gameObject.SetActive(false);
        }
        
        private void ShowMenuButtons()
        {
            foreach (var button in dependencies.upgradeButtons)
                button.button.gameObject.SetActive(true);
            
            foreach (var button in upgradeButtons)
                button.Show();
            
            dependencies.playButton.gameObject.SetActive(true);
        }

        private void StartPlayMode()
        {
            HideMenuButtons();
            ShowCrosshair();
            
            GameEvents.GameStarted.Invoke();
        }

        private void StartMenuMode()
        {
            ShowMenuButtons();
            HideCrosshair();
        }
    }
}