using System;
using UnityEngine;

namespace Extensions
{
    public class TriggerListener : MonoBehaviour
    {
        public event Action<Collider> TriggerEntered;
        public event Action<Collider> TriggerExited;
        
        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke(other);
        }
    }
}