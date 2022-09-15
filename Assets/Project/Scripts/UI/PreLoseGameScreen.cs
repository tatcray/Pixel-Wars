using System;
using Core;
using Dependencies;
using Extensions;
using Saves;
using Weapon;

namespace UI
{
    public class PreLoseGameScreen
    {
        private class MoneyTracker
        {
            private int currentMoney => DataSaveLoader.SerializableData.money.Value;
            private int startingMoney;
            
            public MoneyTracker()
            {
                GameEvents.GameStarted.Event += () => startingMoney = currentMoney;
            }

            public int GetCollectedMoneyOnRound()
            {
                return currentMoney - startingMoney;
            }
        }
        
        private MoneyTracker moneyTracker;
        private UIDependencies dependencies;
        
        public PreLoseGameScreen(UIDependencies uiDependencies)
        {
            moneyTracker = new MoneyTracker();
            dependencies = uiDependencies;
            
            uiDependencies.noThanksButton.onClick.AddListener(SkipAd);
            uiDependencies.viewAdButton.onClick.AddListener(ShowAd);
        }

        public void Show()
        {
            dependencies.collectedMoneyOnRoundMultiplyText.text = (moneyTracker.GetCollectedMoneyOnRound() * 3).ToString();
            dependencies.endGameCanvas.gameObject.SetActive(true);
            dependencies.crosshairCanvas.gameObject.SetActive(false);
        }

        public void Hide()
        {
            dependencies.endGameCanvas.gameObject.SetActive(false);
        }

        private void ShowAd()
        {
            //reward
            DataSaveLoader.SerializableData.money.Value += moneyTracker.GetCollectedMoneyOnRound() * 3;
            Hide();
            GameEvents.GameEndedByLose.Invoke();
        }

        private void SkipAd()
        {
            //inter
            Hide();
            GameEvents.GameEndedByLose.Invoke(); 
        }
    }
}