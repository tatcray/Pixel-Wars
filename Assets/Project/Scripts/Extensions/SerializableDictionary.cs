using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Extensions
{
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public SerializableKeyValuePair()
        {
        }
    }
    
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> data;
        
        public SerializableDictionary() {}

        public SerializableDictionary(SerializableDictionary<TKey, TValue> serializableDictionary)
        {
            foreach (var keyValue in serializableDictionary)
            {
                data.Add(new SerializableKeyValuePair<TKey, TValue>(keyValue.Key, keyValue.Value));
            }
        }

        public SerializableDictionary(SerializationInfo information, StreamingContext context)
            : base(information,context) { }
     
        public void OnBeforeSerialize()
        {
            if (data == null)
                return;
            
            data.Clear();
            
            foreach(KeyValuePair<TKey, TValue> pair in this)
            {
                data.Add(new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
            }
        }
        
        public void OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < data.Count; i++)
            {
                try
                {
                    Add(data[i].Key, data[i].Value);
                }
                catch (ArgumentException argumentException)
                {
                    Add(default, data[i].Value);
                }
            }
        }
    }
}