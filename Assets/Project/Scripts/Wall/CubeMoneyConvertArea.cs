using System.Collections.Generic;
using Core;
using Dependencies;
using Extensions;
using UnityEngine;

namespace Wall
{
    public class CubeMoneyConvertArea
    {
        private class ConvertObjectsPool
        {
            public GameObjectPool attachedPool;
            public float chance;
            public int cost;
        }
        
        private List<ConvertObjectsPool> convertObjectsPools = new List<ConvertObjectsPool>();
        
        private ObservableSerializedObject<int> moneyObject;
        private ConverterDependencies dependencies;
        private CubeGravityCenter cubeGravityCenter;

        private Dictionary<GameObject, GameObjectPool> attachedConvertedObjectsPool = new Dictionary<GameObject, GameObjectPool>();
        private List<GameObject> activeConvertedObjects = new List<GameObject>();
        private ParticlePool particlePool;

        public CubeMoneyConvertArea(ObservableSerializedObject<int> moneyObject, ConverterDependencies converterDependencies)
        {
            this.moneyObject = moneyObject;
            dependencies = converterDependencies;
            
            dependencies.cubeConvertArea.TriggerEntered += RegisterAreaTrigger;

            cubeGravityCenter = new CubeGravityCenter(dependencies.gravityPivot, dependencies.convertAreaGravityForce);

            GameEvents.CubeFalled.Event += cube => cubeGravityCenter.Add(cube);

            particlePool = new ParticlePool(dependencies.convertParticles);

            InitializeConvertObjectsPool();

            GameEvents.GameEndedByLose.Event += DisableAllActiveObjects;
            GameEvents.GameEndedByWin.Event += DisableAllActiveObjects;
        }
        
        private void InitializeConvertObjectsPool()
        {
            foreach (ConvertingObject convertingObject in dependencies.convertingObjects)
            {
                ConvertObjectsPool convertObjectsPool = new ConvertObjectsPool();
                
                convertObjectsPool.chance = convertingObject.convertChance;
                convertObjectsPool.cost = convertingObject.cost;
                convertObjectsPool.attachedPool = new GameObjectPool(convertingObject.gameObject);
                
                convertObjectsPools.Add(convertObjectsPool);
            }
        }

        private void RegisterAreaTrigger(Collider enter)
        {
            Cube cube = CubeTransformGlobalDictionary.Get(enter.transform);
            
            if (cube != null && cube.IsFall())
                ConvertCube(cube);
        }

        private void ConvertCube(Cube cube)
        {
            cube.Hide();
            moneyObject.Value++;

            cubeGravityCenter.Remove(cube);

            if (attachedConvertedObjectsPool.Count > dependencies.maxActiveObjects)
                DisableConvertedObject();
            
            SpawnRandomMoney(cube.GetPosition());
        }

        private void DisableAllActiveObjects()
        {
            int activeObjectsCount = activeConvertedObjects.Count; 
            for (int i = 0; i < activeObjectsCount; i++)
            {
                DisableConvertedObject();
            }
        }

        private void DisableConvertedObject()
        {
            GameObject gameObject = activeConvertedObjects[0];

            GameObjectPool pool = attachedConvertedObjectsPool[gameObject];
            
            pool.Pop(gameObject);
            
            attachedConvertedObjectsPool.Remove(gameObject);
            activeConvertedObjects.Remove(gameObject);
        }

        private void SpawnRandomMoney(Vector3 position)
        {
            ConvertObjectsPool objectsPool = GetRandomPool();
            
            GameObject gameObject = objectsPool.attachedPool.Pull();
                    
            activeConvertedObjects.Add(gameObject);
            attachedConvertedObjectsPool.Add(gameObject, objectsPool.attachedPool);

            moneyObject.Value += objectsPool.cost;

            gameObject.transform.position = position;
            
            particlePool.PlayParticlesOnPosition(position);
        }

        private ConvertObjectsPool GetRandomPool()
        {
            float randomChance = Random.Range(0f, 1f);

            foreach (ConvertObjectsPool objectsPool in convertObjectsPools)
            {
                if (objectsPool.chance > randomChance)
                    return objectsPool;
            }

            return convertObjectsPools[^1];
        }
    }
}