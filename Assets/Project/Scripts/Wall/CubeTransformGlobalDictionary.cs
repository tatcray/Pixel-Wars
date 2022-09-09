using System.Collections.Generic;
using UnityEngine;

namespace Wall
{
    public static class CubeTransformGlobalDictionary
    {
        private static Dictionary<Transform, Cube> cubes = new Dictionary<Transform, Cube>();

        public static void Add(Cube cube, Transform transform) =>
            cubes.Add(transform, cube);

        public static void Remove(Transform transform) =>
            cubes.Remove(transform);

        public static Cube Get(Transform transform) =>
            cubes.ContainsKey(transform) ? cubes[transform] : null;
    }
}