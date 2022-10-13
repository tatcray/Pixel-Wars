using System.Collections.Generic;
using Dependencies;
using Extensions;
using UnityEngine;

namespace Environments
{
    public class CloudsManager
    {
        private Dictionary<Transform, float> cloudsSpeed = new Dictionary<Transform, float>();
        private EnvironmentDependencies dependencies;
        
        public CloudsManager(EnvironmentDependencies dependencies)
        {
            this.dependencies = dependencies;

            SetRandomCloudsSpeed();
            UnityEvents.Update += MoveAllClouds;
        }

        private void MoveAllClouds()
        {
            foreach (var cloudSpeed in cloudsSpeed)
            {
                Transform cloud = cloudSpeed.Key;
                
                cloud.Translate(Vector3.right * cloudSpeed.Value * Time.deltaTime, Space.World);
                
                if (cloud.position.x > dependencies.xEndPoint)
                    MoveCloudToStart(cloud);
            }
        }

        private void MoveCloudToStart(Transform cloud)
        {
            cloud.position = new Vector3(dependencies.xStartPoint, cloud.position.y, cloud.position.z);
        }

        private void SetRandomCloudsSpeed()
        {
            foreach (Transform cloud in dependencies.clouds)
            {
                float speed = dependencies.cloudMoveSpeed;
                speed += Random.Range(-dependencies.cloudRandomMoveSpeedRange, dependencies.cloudRandomMoveSpeedRange);
                cloudsSpeed.Add(cloud, speed);
            }
        }
    }
}