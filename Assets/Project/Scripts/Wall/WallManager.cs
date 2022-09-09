using System.Collections.Generic;
using Core;
using Dependencies;
using UnityEngine;

namespace Wall
{
    public class WallManager
    {
        private static readonly float wallDestroyedPercentToWin = 0.95f;
        
        private List<Cube> activeCubes = new List<Cube>();
        private List<Cube> spawnedCubes = new List<Cube>();
        
        private int cubePixelsResolution;
        private Transform wallPivot;
        private CubeConfig cubeConfig;

        private Texture2D texture;
        private int horizontalCubesCount;
        private int verticalCubesCount;
        private float cubeOffset;

        private int requiredMinActiveCubesToWin;

        public WallManager(WallConfig wallConfig)
        {
            cubePixelsResolution = wallConfig.cubeResolution;
            wallPivot = wallConfig.pivot;
            cubeConfig = wallConfig.cubeConfig;
            GameEvents.CubeFalled.Event += RegisterFalledCube;
        }

        public void DestroyWall()
        {
            for (int i = 0; i < spawnedCubes.Count; i++)
            {
                spawnedCubes[i].DestroyCube();
            }
            
            spawnedCubes.Clear();
            activeCubes.Clear();
        }

        public void SpawnCubes(Sprite sprite)
        {
            cubeOffset = 0;
            texture = sprite.texture;
        
            int pixelsHeight = texture.height;
            int pixelsWidth = texture.width;

            horizontalCubesCount = Mathf.RoundToInt(1.0f * pixelsWidth / cubePixelsResolution);
            verticalCubesCount = Mathf.RoundToInt(1.0f * pixelsHeight / cubePixelsResolution);

            for (int y = 0; y < verticalCubesCount; y++)
                SpawnHorizontalCubesLine(y);

            CalculateActiveCubesToWin();
        }

        private void SpawnHorizontalCubesLine(int y)
        {
            for (int x = 0; x < horizontalCubesCount; x++)
                SpawnAndDrawTextureOnCube(x, y);
        }

        private void SpawnAndDrawTextureOnCube(int x, int y)
        {
            GameObject cubeGameObject = GameObject.Instantiate(cubeConfig.cubePrefab, wallPivot);
        
            cubeGameObject.transform.localPosition = new Vector3(x * cubeOffset, y * cubeOffset);

            SpriteRenderer cubeRenderer = cubeGameObject.GetComponent<SpriteRenderer>();
            SetTextureOnCube(cubeRenderer, x, y);

            if (cubeOffset == 0)
                cubeOffset = cubeRenderer.sprite.bounds.size.x;
            
            Cube cube = new Cube(cubeGameObject, cubeConfig);
            cube.Falled += RegisterFalledCube;
            spawnedCubes.Add(cube);
            activeCubes.Add(cube);
        }

        private void SetTextureOnCube(SpriteRenderer cubeTextureHolder, int x, int y)
        {
            cubeTextureHolder.sprite = CreateSprite(x, y);
        }

        private Sprite CreateSprite(int x, int y)
        {
            Texture2D texture = CreateAdaptedTexture(x, y);
            Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            return Sprite.Create(texture, rect, Vector2.zero, 100f);
        }

        private bool IsAreaOnSpriteEmpty(int x, int y)
        {
            bool isNotAlphaPixelFound = false;
            int horizontalStartPixel = x * cubePixelsResolution;
            int verticalStartPixel = y * cubePixelsResolution;

            for (int vertical = 0; vertical < cubePixelsResolution; vertical++)
            {
                for (int horizontal = 0; horizontal < cubePixelsResolution; horizontal++)
                {
                    Color pixelColor = texture.GetPixel(horizontalStartPixel + horizontal, verticalStartPixel + vertical);
                    if (pixelColor != Color.white && pixelColor.a > 0)
                        return false;
                }
            }

            return true;
        }

        private Texture2D CreateAdaptedTexture(int x, int y)
        {
            Texture2D newTexture = new Texture2D(cubePixelsResolution, cubePixelsResolution);
            int horizontalStartPixel = x * cubePixelsResolution;
            int verticalStartPixel = y * cubePixelsResolution;

            for (int vertical = 0; vertical < cubePixelsResolution; vertical++)
            {
                for (int horizontal = 0; horizontal < cubePixelsResolution; horizontal++)
                {
                    Color pixelColor = texture.GetPixel(horizontalStartPixel + horizontal, verticalStartPixel + vertical);
                    newTexture.SetPixel(horizontalStartPixel + horizontal, verticalStartPixel + vertical, pixelColor);
                }
            }
        
            newTexture.Apply();

            return newTexture;
        }

        private void RegisterFalledCube(Cube cube)
        {
            activeCubes.Remove(cube);
            CheckIsWallDestroyed();
        }

        private void CheckIsWallDestroyed()
        {
            if (activeCubes.Count < requiredMinActiveCubesToWin)
            {
                GameEvents.GameEndedByWin.Invoke();
                Debug.Log("win");
            }
        }

        private void CalculateActiveCubesToWin()
        {
            requiredMinActiveCubesToWin = Mathf.CeilToInt(spawnedCubes.Count * (1 - wallDestroyedPercentToWin));
        }
    }
}
