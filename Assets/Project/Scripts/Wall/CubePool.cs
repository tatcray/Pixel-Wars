using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Wall
{
    public class CubePool
    {
        private List<Cube> cubesPool = new List<Cube>();
        private List<Cube> createdCubes = new List<Cube>();
        private CubeConfig cubeConfig;
        private Transform pivot;
        
        public CubePool(Transform parent, CubeConfig cubeConfig, int startCount = 100)
        {
            this.cubeConfig = cubeConfig;
            
            for (int i = 0; i < startCount; i++)
            {
                IncreasePool();
            }

            pivot = parent;
        }

        public Cube Pull()
        {
            if (cubesPool.Count == 0)
                IncreasePool();

            Cube cube = cubesPool[^1];
            
            cubesPool.Remove(cube);
            
            return cube;
        }

        public void Pop(Cube cube)
        {
            if (cube == null)
                throw new ArgumentNullException();

            if (cubesPool.Contains(cube) || !createdCubes.Contains(cube))
                throw new ArgumentException();
            
            cube.Hide();
            cubesPool.Add(cube);
        }

        private void IncreasePool()
        {
            GameObject cubeGameObject = GameObject.Instantiate(cubeConfig.cubePrefab, pivot);
            cubeGameObject.transform.parent = pivot;
            
            Cube cube = new Cube(cubeGameObject, cubeConfig);
            
            CubeTransformGlobalDictionary.Add(cube, cubeGameObject.transform);
            
            cubesPool.Add(cube);
            createdCubes.Add(cube);

            if (cubeGameObject.transform.parent != pivot)
            {
                Debug.Log("wtf");
            }
        }
    }
}