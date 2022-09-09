using System;
using Extensions;
using UnityEngine;

namespace Wall
{
    [Serializable]
    public class CubeConfig
    {
        public GameObject cubePrefab;
        public float defaultHealth;
        public UnityLayer fallCubeLayer;
        public UnityLayer activeCubeLayer;
    }
}