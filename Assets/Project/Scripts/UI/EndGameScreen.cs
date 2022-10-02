using System;
using Core;
using Dependencies;
using Extensions;
using Saves;
using UnityEngine;
using Weapon;

namespace UI
{
    public class EndGameScreen
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

        private Action onScreenInteractionEnd;
            
        private MoneyTracker moneyTracker;
        private UIDependencies dependencies;
        private int collectableMoney;
        
        public EndGameScreen(UIDependencies uiDependencies)
        {
            moneyTracker = new MoneyTracker();
            dependencies = uiDependencies;
            
            uiDependencies.noThanksButton.onClick.AddListener(SkipAd);
            uiDependencies.viewAdButton.onClick.AddListener(ShowAd);
        }

        public void ShowWinScreen()
        {
            collectableMoney = moneyTracker.GetCollectedMoneyOnRound() * 3;

            Show();

            dependencies.endGameIcon.sprite = dependencies.winIconSprite;
            onScreenInteractionEnd = GameEvents.GameEndedByWin.Invoke;

            dependencies.aboveEndGameIcon.text = "";
        }

        public void ShowLoseScreen()
        {
            collectableMoney = moneyTracker.GetCollectedMoneyOnRound() * 3;

            Show();
            
            dependencies.endGameIcon.sprite = dependencies.loseIconSprite;
            onScreenInteractionEnd = GameEvents.GameEndedByLose.Invoke;
            
            dependencies.aboveEndGameIcon.text = "Ran out of ammo";
        }

        private void Show()
        {
            GameEvents.EndScreenShowed.Invoke();
            
            dependencies.collectedMoneyOnRoundMultiplyText.text = collectableMoney.ToString();
            dependencies.endGameCanvas.gameObject.SetActive(true);
            dependencies.crosshairCanvas.gameObject.SetActive(false);

            UnityEvents.Update += RotateGlow;
        }

        
        private void Hide()
        {
            dependencies.endGameCanvas.gameObject.SetActive(false);
            
            UnityEvents.Update -= RotateGlow;
        }

        private void RotateGlow()
        {
            dependencies.rotatableGlow.rectTransform.Rotate(Vector3.forward, dependencies.rotateGlowSpeed * Time.deltaTime);
        }

        private void ShowAd()
        {
            //reward
            AdController.ShowRewarded(GiveMoneyReward, EndScreenInteraction);
        }

        private void SkipAd()
        {
            AdController.ShowInterstitial(EndScreenInteraction);
        }

        private void EndScreenInteraction()
        {
            Hide();
            onScreenInteractionEnd();
        }

        private void GiveMoneyReward()
        {
            DataSaveLoader.SerializableData.money.Value += collectableMoney;
            EndScreenInteraction();
        }
    }
}