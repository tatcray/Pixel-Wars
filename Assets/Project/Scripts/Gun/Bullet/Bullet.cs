﻿using System;
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
        private ParticleSystem hitEffect;

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

            hitEffect = GameObject.Instantiate(config.hitEffectPrefab.gameObject, parent)
                .GetComponent<ParticleSystem>();
            
            transform = gameObject.transform;

            speed = config.speed;
            lifeTime = config.lifeTime;
            registerLayer = config.registerLayer;
            physicsForce = config.hitPhysicsForce;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            
            UnityEvents.Update += MoveAndTryRegisterHit;
            UnityEvents.Update += LifeTimeUpdate;

            isActive = true;

            previousPosition = Vector3.negativeInfinity;
            activatedTime = 0;
        }

        public void SetDamageAndRadius(float damage, float radius)
        {
            this.damage = damage;
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

            if (previousPosition != Vector3.negativeInfinity && TryRegisterHit())
            {
                hitEffect.transform.position = transform.position;
                hitEffect.Play(true);
                Deactivate();
            }
        }

        private bool TryRegisterHit()
        {
            if (Physics.Linecast(previousPosition, transform.position, registerLayer))
            {
                DamageCubesAround();
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
                    DamageCube(cube);
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
            cube.TakeDamage(damage);
            
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