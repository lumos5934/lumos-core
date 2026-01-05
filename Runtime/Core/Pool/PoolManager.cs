using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib
{
    public class PoolManager : MonoBehaviour, IPreInitializable, IPoolManager
    {
        #region >--------------------------------------------------- FIELD
 
        
        private Dictionary<string, object> _pools = new();
        private Dictionary<string, HashSet<MonoBehaviour>> _activeObjects = new();

        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            GlobalService.Register<IPoolManager>(this);
            return UniTask.FromResult(true);
        }
      
        
        #endregion
        #region >--------------------------------------------------- CREATE

     
        private ObjectPool<T> CreatePool<T>(T prefab, int defaultCapacity = Constant.PoolDefaultCapacity, int maxSize = Constant.PoolMaxSize) where T : MonoBehaviour, IPoolable
        {
            var pool = new ObjectPool<T>(
                createFunc: () =>
                {
                    var obj = Instantiate(prefab);
                    obj.gameObject.SetActive(false);
                    obj.name = prefab.name;
                    return obj;
                },
                actionOnGet: obj =>
                {
                    obj.gameObject.SetActive(true);
                    obj.OnGet();
                },
                actionOnRelease: obj =>
                {
                    obj.OnRelease();
                    obj.gameObject.SetActive(false);
                },
                actionOnDestroy: obj =>
                {
                    if (obj != null)
                    {
                        Destroy(obj.gameObject);
                    }
                },
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );

            _pools.Add(prefab.name, pool);
            return pool;
        }


        #endregion
        #region >--------------------------------------------------- GET


        private ObjectPool<T> GetPool<T>(string key)  where T : MonoBehaviour, IPoolable
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                return pool as ObjectPool<T>;
            }
            
            return null;
        }

        public T Get<T>(T prefab)  where T : MonoBehaviour, IPoolable
        {
            var key = prefab.name;
            var pool = GetPool<T>(key) ?? CreatePool(prefab);

            var obj = pool.Get();

            if (!_activeObjects.ContainsKey(key))
            {
                _activeObjects[key] = new HashSet<MonoBehaviour>();
            }

            _activeObjects[key].Add(obj);

            return obj;
        }


        #endregion
        #region >--------------------------------------------------- REALEASE


        public void Release<T>(T obj)  where T : MonoBehaviour, IPoolable
        {
            var key = obj.name;

            GetPool<T>(key)?.Release(obj);

            if (_activeObjects.TryGetValue(key, out var objs))
            {
                objs.Remove(obj);
            }
        }
        
        public void ReleaseAll<T>(T prefab)  where T : MonoBehaviour, IPoolable
        {
            var key = prefab.name;

            if (_activeObjects.TryGetValue(key, out var objs))
            {
                foreach (var obj in objs)
                {
                    GetPool<T>(key)?.Release(obj as T);
                }
               
                _activeObjects.Remove(key);
            }
        }
   

        #endregion
        #region >--------------------------------------------------- DESTROY

        
        public void DestroyAll<T>(T prefab)  where T : MonoBehaviour, IPoolable
        {
            var key = prefab.name;

            var pool = GetPool<T>(key);
            if (pool != null)
            {
                while (pool.CountInactive > 0)
                {
                    Destroy(pool.Get().gameObject);
                }
                
                pool.Dispose();
                _pools.Remove(key);
            }
            
            if (_activeObjects.TryGetValue(key, out var objs))
            {
                foreach (var obj in objs)
                {
                    Destroy(obj.gameObject);
                }

                _activeObjects.Remove(key);
            }
        }
        
        
        #endregion

      
    }
}