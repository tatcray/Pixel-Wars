using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace Dependencies
{
    [Serializable]
    public class UIDependencies
    {
        public List<UpgradeButtonDependencies> upgradeButtons;
        public List<Sprite> weaponSprites;
        public Button playButton;
        public TextMeshProUGUI money;
        public TextMeshProUGUI ammo;
        public Canvas crosshairCanvas;

        public Canvas endGameCanvas;
        public Button viewAdButton;
        public Button noThanksButton;
        public Image endGameIcon;
        public TextMeshProUGUI aboveEndGameIcon;
        public Sprite loseIconSprite;
        public Sprite winIconSprite;
        public Image rotatableGlow;
        public float rotateGlowSpeed;
        public TextMeshProUGUI collectedMoneyOnRoundMultiplyText;
    }

    [Serializable]
    public class UpgradeButtonDependencies
    {
        public Button button;
        public UpgradeType upgradeType;
        public TextMeshProUGUI cost;
        public Image upgradeBackground;
        public Image icon;
        public Sprite activeUpgradeBackground;
        public Sprite disableUpgradeBackground;
        public Image costIcon;
    }
}