using System;
using System.Collections;
using UnityEngine;

namespace Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static Coroutine InvokeDelay(this MonoBehaviour monoBehaviour, Action method, float delay)
        {
            return monoBehaviour.StartCoroutine(DelayCoroutine());

            IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(delay);
                method();
            }
        }
    }
}