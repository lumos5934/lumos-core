using System;
using UnityEngine;

namespace LumosLib
{
    public class Currency
    {
        public int ID { get; }
        public long Value { get; private set; }

        public event Action<long, long> OnChanged;

        
        public Currency(int id)
        {
            ID = id;
        }

        
        public Currency(int id, long initialValue) : this(id)
        {
            var value = Math.Clamp(initialValue, 0, long.MaxValue);
            
            Value = Math.Max(0, value);
        }

        
        public void Set(long newValue)
        {
            long previous = Value;
            var value = Math.Clamp(newValue, 0, long.MaxValue);
            Value = Math.Max(0, value);

            if (previous != Value)
            {
                OnChanged?.Invoke(previous, Value);
            }
        }

        
        public void Add(long amount)
        {
            Set(Value + amount);
        }
        
        
        public bool Consume(long amount)
        {
            if (Value < amount) 
                return false;
            
            Set(Value - amount);
            
            return true;
        }
    }
}
