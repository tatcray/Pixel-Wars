using System.Collections.Generic;
using Core;
using Extensions;
using UnityEngine;

namespace Wall
{
    public class CubeGravityCenter
    {
        private List<Cube> cubes = new List<Cube>();
        private Transform gravityCenter;
        private float force;
        
        public CubeGravityCenter(Transform gravityCenter, float force)
        {
            this.gravityCenter = gravityCenter;

            this.force = force;
            UnityEvents.Update += DragAllCubesInPoint;
        }

        public void Add(Cube cube) =>
            cubes.Add(cube);

        public void Remove(Cube cube) =>
            cubes.Remove(cube);

        private void DragAllCubesInPoint()
        {
            for (int i = 0; i < cubes.Count; i++)
                cubes[i].AddForce((gravityCenter.position - cubes[i].GetPosition()).normalized * force * Time.deltaTime, ForceMode.Force);
        }
    }
}