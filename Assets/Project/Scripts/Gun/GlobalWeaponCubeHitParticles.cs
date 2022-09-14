using Core;
using Extensions;
using UnityEngine;
using Wall;

namespace Weapon
{
    public class GlobalWeaponCubeHitParticles
    {
        private static GlobalWeaponCubeHitParticles instance;
        private ParticlePool particlePool;
        
        public GlobalWeaponCubeHitParticles(ParticleSystem particles)
        {
            particlePool = new ParticlePool(particles, 15);
            instance = this;
        }

        public static void PlayCubeHitParticles(Vector3 position)
        {
            instance.particlePool.PlayParticlesOnPosition(position);
        }
    }
}