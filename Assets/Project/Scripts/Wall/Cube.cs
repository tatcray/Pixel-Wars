using System;
using Core;
using Extensions;
using UnityEngine;

namespace Wall
{
    public class Cube
    {
        private UnityLayer activeLayer;
        private UnityLayer fallLayer;
        
        private Rigidbody2D rigidbody;
        private Transform transform;
        private GameObject gameObject;
        private SpriteRenderer spriteRenderer;
        
        private float defaultHealth;
        private float health;
        private bool isFall;
        private Vector3 savedPosition;
        
        public Cube(GameObject gameObject, CubeConfig cubeConfig)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            rigidbody = gameObject.GetComponent<Rigidbody2D>();

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

        public float GetSpriteSize()
        {
            return spriteRenderer.sprite.bounds.size.x;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    
        public void Reset()
        {
            transform.position = savedPosition;
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
            }
        }

        public void AddForce(Vector3 force)
        {
            rigidbody.AddForce(force, ForceMode2D.Force);
        }
    
        public void SetDefaultPosition(Vector3 position)
        {
            savedPosition = position;
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
    }
}