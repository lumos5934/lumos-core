using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib
{
    public class PoolManager : BasePoolManager
    {
        #region >--------------------------------------------------- PROPERTIES

        
        public override bool PreInitialized { get; protected set; } = true;

        
        #endregion
        #region >--------------------------------------------------- CREATE


        protected override ObjectPool<T> CreatePool<T>(T prefab, int defaultCapacity = Constant.PoolDefaultCapacity, int maxSize = Constant.PoolMaxSize) 
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

            pools.Add(prefab.name, pool);
            return pool;
        }


        #endregion
        #region >--------------------------------------------------- GET


        protected override ObjectPool<T> GetPool<T>(string key)
        {
            if (pools.TryGetValue(key, out var pool))
            {
                return pool as ObjectPool<T>;
            }
            
            return null;
        }

        public override T Get<T>(T prefab)
        {
            var key = prefab.name;
            var pool = GetPool<T>(key) ?? CreatePool(prefab);

            var obj = pool.Get();

            if (!activeObjects.ContainsKey(key))
            {
                activeObjects[key] = new HashSet<MonoBehaviour>();
            }

            activeObjects[key].Add(obj);

            return obj;
        }


        #endregion
        #region >--------------------------------------------------- REALEASE


        public override void Release<T>(T obj)
        {
            var key = obj.name;

            GetPool<T>(key)?.Release(obj);

            if (activeObjects.TryGetValue(key, out var objs))
            {
                objs.Remove(obj);
            }
        }
        
        public override void ReleaseAll<T>(T prefab)
        {
            var key = prefab.name;

            if (activeObjects.TryGetValue(key, out var objs))
            {
                foreach (var obj in objs)
                {
                    GetPool<T>(key)?.Release(obj as T);
                }
               
                activeObjects.Remove(key);
            }
        }
   

        #endregion
        #region >--------------------------------------------------- DESTROY

        
        public override void DestroyAll<T>(T prefab)
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
                pools.Remove(key);
            }
            
            if (activeObjects.TryGetValue(key, out var objs))
            {
                foreach (var obj in objs)
                {
                    Destroy(obj.gameObject);
                }

                activeObjects.Remove(key);
            }
        }
        
        
        #endregion
    }
}