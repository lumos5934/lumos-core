using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib.Core
{
    public abstract class BasePoolManager : MonoBehaviour, IPoolManager, IPreInitialize
    {
        public int PreID => (int)PreInitializeOrder.Pool;
        public abstract int PreInitOrder { get; }
        public bool PreInitialized { get; private set; }
        
        
        protected Dictionary<string, object> pools = new();
        protected Dictionary<string, HashSet<MonoBehaviour>> activeObjects = new();
        
        
        public virtual void PreInit()
        {
            Global.Register((IPoolManager)this);

            PreInitialized = true;
        }

        protected abstract ObjectPool<T> CreatePool<T>(string key, T prefab, int defaultCapacity, int maxSize) where T : MonoBehaviour, IPoolable;
        public abstract ObjectPool<T> GetPool<T>(T prefab, int defaultCapacity, int maxSize) where T : MonoBehaviour, IPoolable;
        public abstract T Get<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public abstract void Release<T>(T obj) where T : MonoBehaviour, IPoolable;
        public abstract void DestroyActiveObjectsAll();
        public abstract void DestroyActiveObjects<T>(T prefab) where T : MonoBehaviour, IPoolable;
    }
}