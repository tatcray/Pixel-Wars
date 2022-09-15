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
        
        public PreLoseGameScreen(UIDependencies uiDependencies)
        {
            moneyTracker = new MoneyTracker();
        }

        public void ShowScreen()
        {
            
        }
    }
}