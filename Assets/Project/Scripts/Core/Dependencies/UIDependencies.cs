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
        public Button playButton;
        public TextMeshProUGUI money;
        public TextMeshProUGUI ammo;
        public Canvas crosshairCanvas;
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
    }
}