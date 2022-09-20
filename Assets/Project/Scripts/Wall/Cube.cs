using System;
using Core;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Wall
{
    public class Cube
    {
        private UnityLayer activeLayer;
        private UnityLayer fallLayer;
        
        private Rigidbody rigidbody;
        private Transform transform;
        private GameObject gameObject;
        private Material material;
        private MeshRenderer meshRenderer;
        
        private float defaultHealth;
        private float health;
        private bool isFall;
        private float fadeTime;
        private float idleTime;
        private Quaternion savedRotation;
        private Color originalColor;
        private Color damagedColor;
        private Vector3 savedPosition;
        private float colorDamagePercent;

        private Sequence damageAnimationSequence;
        
        public Cube(GameObject gameObject, CubeConfig cubeConfig)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            rigidbody = gameObject.GetComponent<Rigidbody>();

            fadeTime = cubeConfig.damageFadeTime;
            idleTime = cubeConfig.damageIdleTime;
            damagedColor = cubeConfig.targetDamageColor;
            colorDamagePercent = cubeConfig.playingDamageColorPercent;

            defaultHealth = cubeConfig.defaultHealth;
            activeLayer = cubeConfig.activeCubeLayer;
            fallLayer = cubeConfig.fallCubeLayer;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetMaterial(Material material)
        {
            this.material = material;
            originalColor = material.color;
            meshRenderer.material = material;
        }
    
        public void Reset()
        {
            transform.position = savedPosition;
            transform.rotation = savedRotation;
            health = defaultHealth;
            
            SetActive();
            Show();
        }
    
        public bool IsHittable()
        {
            return !isFall;
        }

        public bool IsFall()
        {
            return isFall;
        }
    
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health < 0)
            {
                SetFall();
                PlayDeathAnimation();
            }
        }

        public void AddForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
        {
            rigidbody.AddForce(force, mode);
        }
    
        public void SetDefaultPosition(Vector3 position)
        {
            savedPosition = position;
        }

        public void SetDefaultRotation(Vector3 rotation)
        {
            savedRotation = Quaternion.Euler(rotation);
        }

        private void SetFall()
        {
            EnablePhysics();
            gameObject.layer = fallLayer.LayerIndex;
            isFall = true;
            GameEvents.CubeFalled.Invoke(this);
        }

        private void SetActive()
        {
            DisablePhysics();
            gameObject.layer = activeLayer.LayerIndex;
            isFall = false;
        }

        private void EnablePhysics()
        {
            rigidbody.isKinematic = false;
        }

        private void DisablePhysics()
        {
            gameObject.layer = fallLayer.LayerIndex;
            isFall = false;
            rigidbody.isKinematic = true;
        }

        private void PlayDeathAnimation()
        {
            if(damageAnimationSequence != null)
                damageAnimationSequence.Kill(true);

            Color targetColor = Color.Lerp(originalColor, damagedColor, colorDamagePercent);
            damageAnimationSequence = DOTween.Sequence()
                .Append(material.DOColor(targetColor, fadeTime))
                .AppendInterval(idleTime)
                .Append(material.DOColor(originalColor, fadeTime)).Play();
        }
    }
}