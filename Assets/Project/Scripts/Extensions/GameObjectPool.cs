using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public class GameObjectPool
    {
        private List<GameObject> pooledGameObjects = new List<GameObject>();
        private GameObject poolParent;
        private GameObject objectReference;

        public GameObjectPool(GameObject gameObjectReference, int startCount = 10)
        {
            poolParent = new GameObject($"{gameObjectReference.name} GameObject pool");
            objectReference = gameObjectReference;
            
            for (int i = 0; i < startCount; i++)
                IncreaseGameObjectPool();
        }

        public void Pop(GameObject gameObject)
        {
            pooledGameObjects.Add(gameObject);
        }

        public GameObject Pull()
        {
            if (pooledGameObjects.Count == 0)
                IncreaseGameObjectPool();

            return pooledGameObjects[^1];
        }

        private void IncreaseGameObjectPool()
        {
            GameObject gameObject = GameObject.Instantiate(objectReference, poolParent.transform);
            gameObject.SetActive(false);
            pooledGameObjects.Add(gameObject);
        }

        ~GameObjectPool()
        {
            for (int i = 0; i < pooledGameObjects.Count; i++)
            {
                if (pooledGameObjects[i] != null)
                    GameObject.Destroy(pooledGameObjects[i]);
            }
        }
    }
}