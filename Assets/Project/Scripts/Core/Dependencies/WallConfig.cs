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
        public int cubeResolution;
        public Transform pivot;
        public CubeConfig cubeConfig;
        public TriggerListener cubeConvertArea;
    }
}