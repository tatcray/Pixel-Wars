using System.Collections;
using Dependencies;
using Extensions;
using UnityEngine;

namespace Weapon
{
    public class Weapon
    {
        private Transform transform;
        private GameObject gameObject;
        private Coroutine shootCoroutine;
        private WaitForSeconds waitCorotuine;

        private GameObjectPool slavePool;
        
        private BulletPool bulletPool;
        private Transform bulletReleaseAnchor;
        private ParticleSystem bulletReleaseParticles;
        private Animation shootAnimation;

        private int defaultAmmo;
        private int currentAmmo;
        
        private float damage;
        private float radius;

        public Weapon(WeaponDependency dependency)
        {
            gameObject = dependency.weaponGameObject;
            transform = gameObject.transform;

            bulletReleaseAnchor = dependency.bulletReleaseAnchor;
            bulletReleaseParticles = dependency.bulletReleaseParticles;
            
            bulletPool = new BulletPool(dependency.bulletConfig);
            shootAnimation = dependency.shootAnimation;

            slavePool = new GameObjectPool(dependency.slavePrefab);
        }
        
        public void SetConfig(WeaponConfig config)
        {
            waitCorotuine = new WaitForSeconds(config.fireRate);
            damage = config.damage;
            radius = config.radius;
            defaultAmmo = config.ammo;
        }

        public void Reset()
        {
            currentAmmo = defaultAmmo;
            bulletPool.DeactivateAllActiveBullets();
        }

        public void LookAt(Vector3 target)
        {
            transform.LookAt(target);
        }

        public void StopShoot()
        {
            if (shootCoroutine != null)
                CoroutinesHolder.StopCoroutine(shootCoroutine);
        }

        public void StartShoot()
        {
            if (shootCoroutine != null)
                CoroutinesHolder.StopCoroutine(shootCoroutine);

            shootCoroutine = CoroutinesHolder.StartCoroutine(ShootCoroutine());
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            StopShoot();
            gameObject.SetActive(false);
        }

        protected virtual void ReleaseBullet()
        {
            bulletReleaseParticles.Play(true);
            
            Bullet bullet = bulletPool.Pull();
            bullet.SetPositionAndRotation(bulletReleaseAnchor.position, transform.rotation);
            bullet.SetDamageAndRadius(damage, radius);
            bullet.Activate();
        }

        protected void ReleaseSlave()
        {
            
        } 

        protected IEnumerator ShootCoroutine()
        {
            while(true)
            {
                yield return waitCorotuine;
                shootAnimation.Play();
                ReleaseBullet();
                ReleaseSlave();
            }
        }
        
        protected IEnumerator 
    }
}
