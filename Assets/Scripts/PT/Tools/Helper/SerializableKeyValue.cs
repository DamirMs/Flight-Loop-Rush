using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PT.Tools.Helper
{
    [Serializable]
    public class SerializableKeyValue<TKey, TValue>
    {
        [JsonProperty]
        [SerializeField] private List<TKey> keys = new();
        [JsonProperty]
        [SerializeField] private List<TValue> values = new();
        
        private Dictionary<TKey, TValue> _dictionary;
        public Dictionary<TKey, TValue> Dictionary 
        {
            get
            {
                if (_dictionary == null)
                {
                    _dictionary = new Dictionary<TKey, TValue>();
                    for (int i = 0; i < keys.Count; i++) _dictionary[keys[i]] = values[i];
                }
            
                return _dictionary;
            }
        }
    }
}