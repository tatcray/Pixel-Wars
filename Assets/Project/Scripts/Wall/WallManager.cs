using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Dependencies;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wall
{
    public class WallManager
    {
        public event Action WallSpawned;
        
        public List<Cube> cubes => wallCubes;

        private CubePool cubePool;
        private List<Cube> wallCubes = new List<Cube>();
        
        private Shader materialShader;
        private CubeConfig cubeConfig;

        private Vector3 positionOffset;
        private Texture2D texture;
        private int horizontalCubesCount;
        private int verticalCubesCount;
        private float cubeOffset;
        private float wallRotationRandomRange;

        private int requiredMinActiveCubesToWin;

        public WallManager(WallConfig wallConfig)
        {
            cubeConfig = wallConfig.cubeConfig;
            materialShader = wallConfig.shader;
            cubeOffset = wallConfig.cubeOffset;
            wallRotationRandomRange = wallConfig.yRotationRandomRange;

            positionOffset = wallConfig.pivot.position;
            cubePool = new CubePool(wallConfig.pivot, cubeConfig);
        }

        public void DestroyWall()
        {
            List<Cube> cubesToDeactivate = new List<Cube>(wallCubes);
            foreach (Cube cube in cubesToDeactivate)
            {
                cubePool.Pop(cube);
                wallCubes.Remove(cube);
            }
        }

        public void SpawnCubes(Sprite sprite)
        {
            texture = sprite.texture;

            horizontalCubesCount = texture.width;
            verticalCubesCount = texture.height;

            for (int y = 0; y < verticalCubesCount; y++)
                SpawnHorizontalCubesLine(y);
            
            WallSpawned?.Invoke();
        }

        public void ResetCubes()
        {
            foreach (var cube in cubes)
                cube.Reset();
        }

        private void SpawnHorizontalCubesLine(int y)
        {
            for (int x = 0; x < horizontalCubesCount; x++)
                SpawnAndDrawTextureOnCube(x, y);
        }

        private void SpawnAndDrawTextureOnCube(int x, int y)
        {
            if (IsAreaOnSpriteEmpty(x, y))
                return;
            
            Cube cube = CreateCubeAt(x, y);
            
            wallCubes.Add(cube);
        }

        private bool IsAreaOnSpriteEmpty(int x, int y)
        {
            Color pixelColor = texture.GetPixel(x, y);
            return pixelColor.a == 0;
        }

        private Material CreatePixelMaterial(int x, int y)
        {
            Material material = new Material(materialShader);
            
            Color pixelColor = texture.GetPixel(x, y);
            
            pixelColor.a = 1;
            material.color = pixelColor;

            return material;
        }

        private Cube CreateCubeAt(int x, int y)
        {
            Cube cube = cubePool.Pull();

            Vector3 cubePosition = positionOffset;
            cubePosition.x += x * cubeOffset;
            cubePosition.y += y * cubeOffset;
            cube.SetDefaultPosition(cubePosition);
            
            cube.SetDefaultRotation(GetRandomizedRotationEulers());
            
            cube.SetMaterial(CreatePixelMaterial(x, y));
            cube.Reset();

            return cube;
        }

        private Vector3 GetRandomizedRotationEulers()
        {
            float randomYOffset = Random.Range(-wallRotationRandomRange, wallRotationRandomRange);
            Vector3 rotation = new Vector3(0, 0, randomYOffset);
            return rotation;
        }
    }
}
