using System;
using System.Collections.Generic;

namespace LumosLib
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new();

        
        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var del))
                _events[type] = Delegate.Combine(del, handler);
            else
                _events[type] = handler;
        }

        
        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_events.TryGetValue(type, out var del))
            {
                var result = Delegate.Remove(del, handler);
                if (result == null) _events.Remove(type);
                else _events[type] = result;
            }
        }

        
        public static void Publish<T>(T e)
        {
            if (_events.TryGetValue(typeof(T), out var del))
                ((Action<T>)del)?.Invoke(e);
        }

        
        public static void Clear()
        {
            _events.Clear();
        }
    }
}