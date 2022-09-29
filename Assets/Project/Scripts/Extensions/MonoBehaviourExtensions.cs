using System;
using System.Collections;
using UnityEngine;

namespace Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void InvokeDelay(this MonoBehaviour monoBehaviour, Action method, float delay)
        {
            monoBehaviour.StartCoroutine(DelayCoroutine());

            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method();
            }
        }
    }
}