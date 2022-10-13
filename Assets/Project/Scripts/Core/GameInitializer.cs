using System.Collections;
using Controller;
using Environments;
using Extensions;
using Saves;
using Services;
using Tutorial;
using UI;
using Unity.Collections;
using UnityEngine;
using Upgrades;
using Wall;
using Weapon;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private DependenciesData dependencies;
        [SerializeField]
        private SerializableDataSave save;
        
        private int loadedWallIndex;
        private WallManager wallManager;
        private Crosshair crosshair;
        private UpgradeSystem upgradeSystem;
        private CameraCornerFollower cornerFollower;
        private WallDestroyingObserver wallDestroyingObserver;
        private WeaponManager weaponManager;
        private GameScreen gameScreen;
        private EndGameScreen endGameScreen;
        private TutorialManager tutorialManager;
        private AdController adController;
        
        private void Start()
        {
            InitializeEnvironment();
            
            InitializeSaves();
            InitializeCubes();
            InitializeCrosshair();
            InitializeWeapon();
            InitializeUpgrades();
            InitializeCamera();

            InitializeUI();
            InitializeTutorial();
            
            InitializeAnalyticsEvents();
            InitializeAds();
            InitializeGameEvents();
        }

        private void InitializeEnvironment()
        {
            new CloudsManager(dependencies.environmentDependencies);
        }

        private void InitializeSaves()
        {
            DataSaveLoader saveLoader = new DataSaveLoader();
            save = saveLoader.LoadData();
        }

        private void InitializeCrosshair()
        {
            crosshair = new Crosshair(dependencies.crosshairReferences);
        }

        private void InitializeWeapon()
        {
            weaponManager = new WeaponManager(dependencies.weaponReferences, crosshair);
        }

        private void InitializeUpgrades()
        {
            upgradeSystem = new UpgradeSystem(dependencies.upgradesData, weaponManager, save.upgrades);
        }

        private void InitializeCamera()
        {
            cornerFollower = new CameraCornerFollower(dependencies.cameraDependencies);
            cornerFollower.SaveCurrentPositionAsDefault();
        }

        private void InitializeCubes()
        {
            wallManager = new WallManager(dependencies.wallConfig);
            wallDestroyingObserver = new WallDestroyingObserver(wallManager);

            LoadWallFromSave();

            new CubeMoneyConvertArea(save.money, dependencies.converterDependencies);
        }

        private void InitializeUI()
        {
            gameScreen = new GameScreen(dependencies.uiDependencies, upgradeSystem);

            weaponManager.ammo.DataChanged += gameScreen.SetAmmo;
            save.money.DataChanged += gameScreen.SetMoney;
            save.wallIndex.DataChanged += gameScreen.SetNewLevelOnProgressBar;
            
            gameScreen.SetMoney(save.money.Value);
            gameScreen.SetAmmo(weaponManager.ammo.Value);
            gameScreen.SetNewLevelOnProgressBar(save.wallIndex.Value);

            endGameScreen = new EndGameScreen(dependencies.uiDependencies);
        }

        private void InitializeTutorial()
        {
            tutorialManager = new TutorialManager(save.tutorialIndex);
            tutorialManager.AddTutorialPart(new ButtonTutorial(dependencies.tutorialDependencies, dependencies.tutorialDependencies.playButton));
            tutorialManager.AddTutorialPart(new ShootTutorial(dependencies.tutorialDependencies, crosshair));
            tutorialManager.AddTutorialPart(new UpgradeTutorial(dependencies.tutorialDependencies, save.money));
            
            if (!tutorialManager.IsFinished())
                tutorialManager.StartTutorial();
        }

        private void InitializeAnalyticsEvents()
        {
            GameEvents.GameEndedByLose.Event += AnalyticsController.SendLevelFailEvent;
            GameEvents.GameEndedByWin.Event += AnalyticsController.SendLevelCompletedEvent;

            CoroutinesHolder.StartCoroutine(SendAnalyticsTimeEvents());
        }
        
        private void InitializeAds()
        {
            adController = new AdController(dependencies.servicesDependencies);
            adController.InitializeSdk();
        }
 
        private void InitializeGameEvents()
        {
            
            GameEvents.CubeFalled.Event += cube => cornerFollower.TrySetNewCorner(cube.GetPosition());
            
            GameEvents.CubeFalled.Event += wallDestroyingObserver.RegisterCubeFall;
            
            GameEvents.GameEndedByLose.Event += cornerFollower.ResetToDefaultPosition;
            GameEvents.GameEndedByLose.Event += wallManager.ResetCubes;
            GameEvents.GameEndedByLose.Event += wallDestroyingObserver.Reset;
            
            GameEvents.GameEndedByWin.Event += () => save.wallIndex.Value++;
            GameEvents.GameEndedByWin.Event += cornerFollower.ResetToDefaultPosition;
            GameEvents.GameEndedByWin.Event += wallDestroyingObserver.Reset;
            
            save.wallIndex.DataChanged += newWallIndex =>
            {
                if (newWallIndex != loadedWallIndex)
                    LoadWallFromSave();
            };
            
            AmmoTracker ammoTracker = new AmmoTracker(weaponManager.playingBullets);
            ammoTracker.AmmoEnded += () => this.InvokeDelay(endGameScreen.ShowLoseScreen, 0.4f);
            ammoTracker.StartTracking();
            GameEvents.EndScreenShowed.Event += ammoTracker.StopTracking;
            GameEvents.GameStarted.Event += ammoTracker.StartTracking;
            
            wallDestroyingObserver.WallDestroyed += endGameScreen.ShowWinScreen;
            wallDestroyingObserver.WallDestroyPercentUpdated += gameScreen.SetNewProgressBarValue;
        }

        private void LoadWallFromSave()
        {
            wallManager.DestroyWall();

            Sprite sprite = dependencies.wallConfig.sprites[save.wallIndex.Value % dependencies.wallConfig.sprites.Count];
            wallManager.SpawnCubes(sprite);
        }

        private IEnumerator SendAnalyticsTimeEvents()
        {
            AnalyticsController.SendSessionStartPlay(save.timeIndex.Value);
            WaitForSeconds interval = new WaitForSeconds(dependencies.servicesDependencies.analyticsSecondsTimeInterval);

            int sessionTimeIndex = 0;

            while (true)
            {
                yield return interval;
                save.timeIndex.Value++;
                sessionTimeIndex++;

                AnalyticsController.SendPlayedSessionTime(sessionTimeIndex);
                AnalyticsController.SendPlayedTime(save.timeIndex.Value);
            }
        }
    }
}