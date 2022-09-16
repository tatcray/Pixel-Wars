using System;
using Core;
using Extensions;
using UnityEngine;
using Wall;

namespace Weapon
{
    public class Bullet
    {
        public event Action Deactivated;

        public GameObject gameObject { get; }

        private bool isActive;
        private Transform transform;

        private float speed;
        private float activatedTime;
        private float lifeTime;
        private float physicsForce;
        private LayerMask registerLayer;

        private Vector3 previousPosition;

        private float radius;
        private float damage;

        public Bullet(BulletConfig config, Transform parent)
        {
            gameObject = GameObject.Instantiate(config.prefab, parent);
            gameObject.SetActive(false);
            
            transform = gameObject.transform;

            speed = config.speed;
            lifeTime = config.lifeTime;
            damage = config.damage;
            registerLayer = config.registerLayer;
            physicsForce = config.hitPhysicsForce;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            
            UnityEvents.Update += MoveAndTryRegisterHit;
            UnityEvents.Update += LifeTimeUpdate;

            isActive = true;

            activatedTime = 0;
        }

        public void SetRadius(float radius)
        {
            this.radius = radius;
        }

        public void SetPositionAndRotation(Vector3 position, Vector3 rotation)
        {
            position.z = 0;

            rotation.y = 90;
            rotation.z = 0;
            Quaternion quaternion = Quaternion.Euler(rotation);
            transform.SetPositionAndRotation(position, quaternion);
        }

        public void Deactivate()
        {
            if (!isActive)
                return;

            gameObject.SetActive(false);

            isActive = false;

            UnityEvents.Update -= MoveAndTryRegisterHit;
            UnityEvents.Update -= LifeTimeUpdate;

            Deactivated?.Invoke();
        }

        private void LifeTimeUpdate()
        {
            activatedTime += Time.deltaTime;
            if (activatedTime > lifeTime)
                Deactivate();
        }

        private void MoveAndTryRegisterHit()
        {
            if (!isActive)
                return;
            
            MoveBullet();

            if (TryRegisterHit())
            {
                Deactivate();
            }
        }

        private bool TryRegisterHit()
        {
            if (Physics.Linecast(previousPosition, transform.position, registerLayer))
            {
                DamageCubesAround();
                
                GlobalWeaponCubeHitParticles.PlayCubeHitParticles(transform.position);
                return true;
            }

            return false;

        }

        private void DamageCubesAround()
        {
            Collider[] registeredColliders = Physics.OverlapSphere(transform.position, radius, registerLayer);
            foreach (var collider in registeredColliders)
            {
                if (!isActive)
                    return;
                
                Cube cube = CubeTransformGlobalDictionary.Get(collider.transform);

                if (cube != null && cube.IsHittable())
                {
                    DamageCube(cube);
                }
            }
        }

        private void MoveBullet()
        {
            previousPosition = transform.position;
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }

        private void DamageCube(Cube cube)
        {
            float damage = CalculateDamage(cube);
            if (damage > 0)
            {
                cube.TakeDamage(damage);
            }
            
            if (cube.IsFall())
                AddForceOnCube(cube);
        }

        private float CalculateDamage(Cube cube)
        {
            float distance = Vector3.Distance(transform.position, cube.GetPosition());

            if (distance > radius)
                return 0;

            return damage - damage * (distance / radius);
        }

        private void AddForceOnCube(Cube cube)
        {
            cube.AddForce((cube.GetPosition() - transform.position + Vector3.up * 0.3f).normalized * physicsForce);
        }
    }
}