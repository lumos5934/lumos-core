using System;
using System.Collections.Generic;

namespace LumosLib
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _table = new ();


        public EventBus()
        {
            GlobalService.Register<IEventBus>(this);
        }
        
        public void Subscribe<T>(Action<T> listener)
        {
            var type = typeof(T);

            if (!_table.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _table[type] = list;
            }

            list.Add(listener);
        }

        
        public void Unsubscribe<T>(Action<T> listener)
        {
            var type = typeof(T);

            if (_table.TryGetValue(type, out var list))
            {
                list.Remove(listener);

                if (list.Count == 0)
                    _table.Remove(type);
            }
        }

        
        public void Publish<T>(T evt)
        {
            var type = typeof(T);

            if (_table.TryGetValue(type, out var list))
            {
                var temp = list.ToArray();

                foreach (var del in temp)
                {
                    (del as Action<T>)?.Invoke(evt);
                }
            }
        }
    }
}