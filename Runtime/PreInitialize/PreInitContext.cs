using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LumosLib
{
    public class PreInitContext
    {
        private readonly Dictionary<Type, IPreInitializable> _instances = new();
        private readonly Dictionary<Type, UniTask<bool>> _tasks = new();

        
        public void Register(IPreInitializable instance, UniTask<bool> task)
        {
            var type = instance.RegisterType;
            _instances[type] = instance;
            _tasks[type] = task;
        }

        
        public async UniTask<T> GetAsync<T>() where T : class
        {
            if (_tasks.TryGetValue(typeof(T), out var task))
            {
                bool success = await task;
                if (success)
                {
                    return _instances[typeof(T)] as T;
                }
            }
            
            return null;
        }
    }
}