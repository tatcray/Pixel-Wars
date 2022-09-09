using System;
using UnityEngine;

namespace Extensions
{
    [Serializable]
    public class ObservableSerializedObject<T>
    {
        public event Action<T> DataChanged;

        [SerializeField]
        private T value;

        public T Value
        {
            get
            {
                return value;
            }
            
            set 
            { 
                this.value = value; 
                DataChanged?.Invoke(value); 
            }
        }
    }
}