using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib
{
    public abstract class BasePoolManager : MonoBehaviour, IPreInitializer, IPoolManager
    {
        #region >--------------------------------------------------- PROPERTIE
        
        public int PreInitOrder  => (int)PreInitializeOrder.Pool;
        
        
        #endregion
        #region >--------------------------------------------------- FIELD
 
        
        protected Dictionary<string, object> pools = new();
        protected Dictionary<string, HashSet<MonoBehaviour>> activeObjects = new();

        
        #endregion
        #region >--------------------------------------------------- UNITY

        
        protected void Awake()
        {
            GlobalService.Register<IPoolManager>(this);
            
            DontDestroyOnLoad(gameObject);
        }
        

        #endregion
        #region >--------------------------------------------------- INIT


        public abstract IEnumerator InitAsync();
       
        
        #endregion
        #region >--------------------------------------------------- CORE

        
        protected abstract ObjectPool<T> CreatePool<T>(T prefab, int defaultCapacity, int maxSize) where T : MonoBehaviour, IPoolable;
        protected abstract ObjectPool<T> GetPool<T>(string key) where T : MonoBehaviour, IPoolable;
        public abstract T Get<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public abstract void Release<T>(T obj) where T : MonoBehaviour, IPoolable;
        public abstract void ReleaseAll<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public abstract void DestroyAll<T>(T prefab) where T : MonoBehaviour, IPoolable;
        
        
        #endregion
    }
}