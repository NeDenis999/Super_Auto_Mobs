using System;
using UnityEngine;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct SerializablePair<TKey, TValue>
    {
        public SerializablePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        
        [field: SerializeField] 
        public TKey Key { get; private set; }
        
        [field: SerializeField]
        public TValue Value { get; private set; }
    }
}