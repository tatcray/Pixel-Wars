using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dependencies
{
    [Serializable]
    public class EnvironmentDependencies
    {
        public List<Transform> clouds;
        public float cloudMoveSpeed;
        public float cloudRandomMoveSpeedRange;
        public float xStartPoint;
        public float xEndPoint;
    }
}