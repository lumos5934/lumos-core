using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LumosLib
{
    public class PreInitContext
    {
        private readonly Dictionary<Type, IPreInitializable> _instances = new();
        private readonly Dictionary<Type, UniTaskCompletionSource<UniTask<bool>>> _taskSources = new();

        private UniTaskCompletionSource<UniTask<bool>> GetSource(Type type)
        {
            if (!_taskSources.TryGetValue(type, out var tcs))
            {
                tcs = new UniTaskCompletionSource<UniTask<bool>>();
                _taskSources[type] = tcs;
            }
            return tcs;
        }

        public void Register(IPreInitializable instance, UniTask<bool> task)
        {
            _instances[instance.RegisterType] = instance;
            // 작업이 시작되었음을 알리고 대기표(task)를 넘겨줌
            GetSource(instance.RegisterType).TrySetResult(task);
        }

        public async UniTask<T> GetAsync<T>() where T : class
        {
            var type = typeof(T);
            // 작업(Task)이 들어올 때까지 기다림
            var task = await GetSource(type).Task;
            bool success = await task; 

            if (success && _instances.TryGetValue(type, out var instance))
                return instance as T;

            return null;
        }
    }
}