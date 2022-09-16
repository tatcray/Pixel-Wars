using System;
using Core;
using UnityEngine;

namespace Wall
{
    public class WallDestroyingObserver
    {
        public event Action WallDestroyed;
        private static readonly float wallDestroyedPercentToWin = 0.95f;
        private int minActiveCubesToDestroyed;
        private int activeCubes;
        private int totalWallCubesCount;
        private WallManager wallManager;
        
        public WallDestroyingObserver(WallManager wallManager)
        {
            this.wallManager = wallManager;
            wallManager.WallSpawned += UpdateTotalCubesCount;
        }
        
        public void RegisterCubeFall(Cube cube)
        {
            activeCubes--;
            
            if (IsWallDestroyed())
                WallDestroyed?.Invoke();
        }
        
        public void ResetActiveCubesCount()
        {
            activeCubes = totalWallCubesCount;
        }

        private bool IsWallDestroyed()
        {
            return activeCubes < minActiveCubesToDestroyed;
        }

        private void UpdateTotalCubesCount()
        {
            totalWallCubesCount = wallManager.cubes.Count;
            CalculateActiveCubesToWin();
            ResetActiveCubesCount();
        }

        private void CalculateActiveCubesToWin()
        {
            minActiveCubesToDestroyed = Mathf.CeilToInt(totalWallCubesCount * (1 - wallDestroyedPercentToWin));
        }
    }
}