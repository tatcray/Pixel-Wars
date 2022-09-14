using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public class ParticlePool
    {
        private List<ParticleSystem> particlesPool = new List<ParticleSystem>();
        private ParticleSystem particlePrefab;
        private Transform parent;
        
        public ParticlePool(ParticleSystem particleSystem, int poolCount = 10)
        {
            particlePrefab = particleSystem;
            parent = new GameObject($"{particleSystem.gameObject.name} particles pool").transform;
            
            for (int i = 0; i < poolCount; i++)
                IncreasePool();
        }

        public ParticleSystem Pull()
        {
            foreach (ParticleSystem particleSystem in particlesPool)
            {
                if (!particleSystem.isPlaying)
                    return particleSystem;
            }

            return IncreasePool();
        }

        public void PlayParticlesOnPosition(Vector3 position)
        {
            ParticleSystem particleSystem = Pull();

            particleSystem.transform.position = position;
            
            particleSystem.Play();
        }

        private ParticleSystem IncreasePool()
        {
            ParticleSystem particleSystem = ParticleSystem.Instantiate(particlePrefab, parent);
            particlesPool.Add(particleSystem);

            return particleSystem;
        }
    }
}