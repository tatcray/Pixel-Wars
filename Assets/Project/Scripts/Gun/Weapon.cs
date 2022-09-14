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
        private WaitForSeconds shootWaitCoroutine;
        private WaitForSeconds slaveWaitCoroutine;

        private GameObjectPool slavePool;
        
        private BulletPool bulletPool;
        private Transform bulletReleaseAnchor;
        private Transform slaveAnchor;
        private ParticleSystem bulletReleaseParticles;
        private Animation shootAnimation;
        private Vector3 slaveReleaseForce;
        
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
            slaveWaitCoroutine = new WaitForSeconds(dependency.slaveLifeTime);
            slaveReleaseForce = dependency.slaveForce;
            
            bulletPool = new BulletPool(dependency.bulletConfig);
            slaveAnchor = dependency.weaponSlaveAnchor;
            shootAnimation = dependency.shootAnimation;

            slavePool = new GameObjectPool(dependency.slavePrefab);
        }
        
        public void SetConfig(WeaponConfig config)
        {
            shootWaitCoroutine = new WaitForSeconds(config.fireRate);
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
            bullet.SetPositionAndRotation(bulletReleaseAnchor.position, bulletReleaseAnchor.rotation.eulerAngles);
            bullet.SetRadius(radius);
            bullet.Activate();
        }

        protected void ReleaseSlave()
        {
            GameObject slave = slavePool.Pull();
            slave.transform.rotation = slaveAnchor.rotation;
            slave.transform.position = slaveAnchor.position;

            Rigidbody slaveRigidBody = slave.GetComponent<Rigidbody>();
            slaveRigidBody.velocity = Vector3.zero;
            slaveRigidBody.AddForce(slaveReleaseForce, ForceMode.Impulse);
            
            CoroutinesHolder.StartCoroutine(SlaveDisappearCoroutine(slave));
        } 

        protected IEnumerator ShootCoroutine()
        {
            while(true)
            {
                yield return shootWaitCoroutine;
                shootAnimation.Play();
                ReleaseBullet();
                ReleaseSlave();
            }
        }

        protected IEnumerator SlaveDisappearCoroutine(GameObject slave)
        {
            yield return slaveWaitCoroutine;
            slavePool.Pop(slave);
        }
    }
}
