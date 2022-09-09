using System.Collections;
using UnityEngine;

public static class CoroutinesHolder
{
    private static readonly string gameObjectName = "CoroutineHolder";
    private static MonoBehaviourSample coroutineHolder;

    static CoroutinesHolder()
    {
        coroutineHolder = new GameObject(gameObjectName).AddComponent<MonoBehaviourSample>();
    }
    
    public static Coroutine StartCoroutine(IEnumerator routine) =>
        coroutineHolder.StartCoroutine(routine);

    public static void StopCoroutine(Coroutine routine) =>
        coroutineHolder.StopCoroutine(routine);

    public static void StopAllCoroutines() =>
        coroutineHolder.StopAllCoroutines();
    
    private class MonoBehaviourSample : MonoBehaviour { }
}