using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using Wall;

namespace Dependencies
{
    [Serializable]
    public class WallConfig
    {
        public List<Sprite> sprites;
        public Transform pivot;
        public Shader shader;
        public float cubeOffset;
        public float yRotationRandomRange;
        public CubeConfig cubeConfig;
        public TriggerListener cubeConvertArea;
    }
}