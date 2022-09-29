using System;
using System.Collections;
using Dependencies;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapon
{
    public class Weapon
    {
        public event Action Shooted;
        public ObservableSerializedObject<int> flyingBullets => bulletPool.activeBulletsCount;

        private Transform transform;
        private GameObject gameObject;
        private Coroutine shootCoroutine;
        private WaitForSeconds shootWaitCoroutine;
        private WaitForSeconds slaveWaitCoroutine;

        private GameObjectPool slavePool;
        
        private BulletPool bulletPool;

        private WeaponDependency dependency;
        
        private float damage;
        private float radius;

        public Weapon(WeaponDependency dependency)
        {
            gameObject = dependency.weaponGameObject;
            transform = gameObject.transform;
            
            this.dependency = dependency;
            
            slaveWaitCoroutine = new WaitForSeconds(dependency.slaveLifeTime);
            bulletPool = new BulletPool(dependency.bulletConfig);
            slavePool = new GameObjectPool(dependency.slavePrefab);
        }
        
        public void SetConfig(WeaponConfig config)
        {
            shootWaitCoroutine = new WaitForSeconds(config.fireRate);
            damage = config.damage;
            radius = config.radius;
        }

        public void Reset()
        {
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
            dependency.bulletReleaseParticles.Play(true);
            
            Bullet bullet = bulletPool.Pull();
            bullet.SetPositionAndRotation(dependency.bulletReleaseAnchor.position, dependency.bulletReleaseAnchor.rotation.eulerAngles);
            bullet.SetRadius(radius);
            bullet.Activate();
        }

        protected void ReleaseSlave()
        {
            GameObject slave = slavePool.Pull();
            slave.transform.position = dependency.weaponSlaveAnchor.position;

            Rigidbody slaveRigidBody = slave.GetComponent<Rigidbody>();
            slaveRigidBody.velocity = Vector3.zero;
            float multiplier = Random.Range(1f, dependency.maxForceMultiplier);
            slaveRigidBody.AddForce(dependency.slaveForce * multiplier, ForceMode.Impulse);
            
            CoroutinesHolder.StartCoroutine(SlaveDisappearCoroutine(slave));
        } 

        protected IEnumerator ShootCoroutine()
        {
            while(true)
            {
                yield return shootWaitCoroutine;
                dependency.shootAnimation.Play();
                Shooted?.Invoke();
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
