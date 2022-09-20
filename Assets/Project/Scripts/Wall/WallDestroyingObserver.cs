using System;
using Core;
using UnityEngine;

namespace Wall
{
    public class WallDestroyingObserver
    {
        public event Action<float> WallDestroyPercentUpdated;
        public event Action WallDestroyed;
        private static readonly float wallDestroyedPercentToWin = 0.95f;
        private int minActiveCubesToDestroyed;
        private int activeCubes;
        private int totalWallCubesCount;
        private WallManager wallManager;
        private bool isDestroyed;
        
        public WallDestroyingObserver(WallManager wallManager)
        {
            this.wallManager = wallManager;
            wallManager.WallSpawned += UpdateTotalCubesCount;
        }

        public void RegisterCubeFall(Cube cube)
        {
            if (isDestroyed)
                return;

            activeCubes--;

            float destroyPercent = GetDestroyPercent() / wallDestroyedPercentToWin;
            if (destroyPercent >= 1)
            {
                isDestroyed = true;
                WallDestroyed?.Invoke();
            }
            
            WallDestroyPercentUpdated?.Invoke(destroyPercent);
        }

        public void Reset()
        {
            activeCubes = totalWallCubesCount;
            isDestroyed = false;
        }

        private float GetDestroyPercent()
        {
            return  1 - (float)activeCubes / (float)totalWallCubesCount;
        }

        private void UpdateTotalCubesCount()
        {
            totalWallCubesCount = wallManager.cubes.Count;
            Reset();
        }
    }
}