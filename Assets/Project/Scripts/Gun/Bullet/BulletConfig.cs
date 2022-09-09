using System;
using Extensions;
using UnityEngine;

namespace Weapon
{
    [Serializable]
    public class BulletConfig
    {
        public ParticleSystem hitEffectPrefab;
        public float lifeTime;
        public float speed;
        public GameObject prefab;
        public float hitPhysicsForce;
        public LayerMask registerLayer;
    }
}