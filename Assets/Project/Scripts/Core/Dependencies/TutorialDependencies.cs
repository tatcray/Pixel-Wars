using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dependencies
{
    [Serializable]
    public class TutorialDependencies
    {
        public Image handContainer;
        public Sprite pressedHand;
        public Sprite idleHand;
        public int pixelsHandWalkRange;
        public float handMoveDuration;
        public float handIdleDuration;
        public float handMoveDelay;

        public Button playButton;
        public Button upgradeButton;
        public List<Button> buttonsToDisableOnUpgradePart;
        public int minimalMoneyOnUpgradePart;
        public float fadeDuration;
        public float clickDelay;
    }
}