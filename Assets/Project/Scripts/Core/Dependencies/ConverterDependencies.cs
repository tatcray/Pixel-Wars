using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Dependencies
{
    [Serializable]
    public class ConverterDependencies
    {
        public TriggerListener cubeConvertArea;
        public Transform gravityPivot;
        public float convertAreaGravityForce;
        public List<ConvertingObject> convertingObjects;
        public int maxActiveObjects;
        public ParticleSystem convertParticles;
    }
    
    [Serializable]
    public struct ConvertingObject
    {
        public GameObject gameObject;
        public float convertChance;
        public int cost;
    }
}