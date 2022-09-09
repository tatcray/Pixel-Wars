using System;
using System.Collections.Generic;
using Dependencies;
using Extensions;
using UnityEngine;

namespace Weapon
{
    public class BulletPool
    {
        private GameObject poolParent;
        
        private BulletConfig bulletConfig;
        private List<Bullet> bulletPool = new List<Bullet>();
        private List<Bullet> activeBullets = new List<Bullet>();
        private List<Bullet> createdBullets = new List<Bullet>();
        
        public BulletPool(BulletConfig config, int startCount = 20)
        {
            bulletConfig = config;
            poolParent = new GameObject($"{config.prefab.name} pool");
            
            for (int i = 0; i < startCount; i++)
                IncreaseGameObjectPool();
        }

        public Bullet Pull()
        {
            if (bulletPool.Count == 0)
                IncreaseGameObjectPool();

            Bullet bullet = bulletPool[^1];
            
            bulletPool.Remove(bullet);
            activeBullets.Add(bullet);
            
            return bullet;
        }

        public void Pop(Bullet bullet)
        {
            if (bullet == null)
                throw new ArgumentNullException();

            if (bulletPool.Contains(bullet) || !createdBullets.Contains(bullet))
                throw new ArgumentException();
            
            bulletPool.Add(bullet);
            activeBullets.Remove(bullet);
        }

        public void DeactivateAllActiveBullets()
        {
            List<Bullet> bulletsToDeactivate = new List<Bullet>(activeBullets);
            foreach (Bullet bullet in bulletsToDeactivate)
                bullet.Deactivate();
        }
        
        private void IncreaseGameObjectPool()
        {
            Bullet newBullet = new Bullet(bulletConfig, poolParent.transform);

            newBullet.Deactivated += () => Pop(newBullet);
            
            bulletPool.Add(newBullet);
            createdBullets.Add(newBullet);
        }
    }
}