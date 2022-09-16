using Controller;
using Environment;
using Extensions;
using Saves;
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
            
            gameScreen.SetMoney(save.money.Value);
            gameScreen.SetAmmo(weaponManager.ammo.Value);

            endGameScreen = new EndGameScreen(dependencies.uiDependencies);
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
            
            AmmoTracker ammoTracker = new AmmoTracker(weaponManager.ammo);
            ammoTracker.AmmoEnded += endGameScreen.ShowLoseScreen;
            wallDestroyingObserver.WallDestroyed += endGameScreen.ShowWinScreen;
        }

        private void LoadWallFromSave()
        {
            wallManager.DestroyWall();

            Sprite sprite = dependencies.wallConfig.sprites[save.wallIndex.Value % dependencies.wallConfig.sprites.Count];
            wallManager.SpawnCubes(sprite);
        }
    }
}