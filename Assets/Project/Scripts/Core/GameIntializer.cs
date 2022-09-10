﻿using Controller;
using Extensions;
using Saves;
using Unity.Collections;
using UnityEngine;
using Wall;
using Weapon;

namespace Core
{
    public class GameIntializer : MonoBehaviour
    {
        [SerializeField]
        private DependenciesData dependencies;
        [SerializeField]
        private SerializableDataSave save;
        
        private int loadedWallIndex;
        private WallManager wallManager;
        private Crosshair crosshair;
        private CameraCornerFollower cornerFollower;
        private WallDestroyingObserver wallDestroyingObserver;
        private WeaponManager weaponManager;
        
        private void Start()
        {
            InitializeSaves();
            InitializeCubes();
            InitializeCrosshair();
            InitializeWeapon();
            InitializeCamera();
            
            InitializeGameEvents();
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
            weaponManager.LoadWeapon(WeaponType.Glock, new WeaponConfig() {ammo = 10, fireRate = 0.6f, radius = 2f, damage = 15f});
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
            
            save.wallIndex.DataChanged += newWallIndex =>
            {
                if (newWallIndex != loadedWallIndex)
                    LoadWallFromSave();
            };
            
            LoadWallFromSave();

            new CubeMoneyConvertArea(save.money, dependencies.wallConfig.cubeConvertArea);
        }

        private void InitializeGameEvents()
        {
            GameEvents.GameEndedByWin.Event += () => save.wallIndex.Value++;
            GameEvents.CubeFalled.Event += cube => cornerFollower.TrySetNewCorner(cube.GetPosition());
            GameEvents.GameEndedByLose.Event += cornerFollower.ResetToDefaultPosition;
            GameEvents.GameEndedByWin.Event += cornerFollower.ResetToDefaultPosition;
            
            GameEvents.CubeFalled.Event += wallDestroyingObserver.RegisterCubeFall;
            GameEvents.GameEndedByLose.Event += wallDestroyingObserver.ResetActiveCubesCount;
            GameEvents.GameEndedByWin.Event += wallDestroyingObserver.ResetActiveCubesCount;
        }

        private void LoadWallFromSave()
        {
            wallManager.DestroyWall();

            Sprite sprite = dependencies.wallConfig.sprites[save.wallIndex.Value % dependencies.wallConfig.sprites.Count];
            wallManager.SpawnCubes(sprite);
        }
    }
}