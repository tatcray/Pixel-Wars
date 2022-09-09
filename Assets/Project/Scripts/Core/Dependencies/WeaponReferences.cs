using System;
using Extensions;
using UnityEngine;
using Weapon;

namespace Dependencies
{
    [Serializable]
    public class WeaponReferences
    {
        public SerializableDictionary<WeaponType, WeaponDependency> weapons;
    }
    
    [Serializable]
    public class WeaponDependency
    {
        public Animation shootAnimation;
        public Transform weaponSlaveAnchor;
        public Vector3 slaveForce;
        public float slaveLifeTime;
        public ParticleSystem bulletReleaseParticles;
        public Transform bulletReleaseAnchor;
        public GameObject weaponGameObject;
        public GameObject slavePrefab;
        public BulletConfig bulletConfig;
    }
}