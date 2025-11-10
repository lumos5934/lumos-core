using System.Collections.Generic;
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


        protected override ObjectPool<T> CreatePool<T>(string key, T prefab, int defaultCapacity = Constant.PoolDefaultCapacity, int maxSize = Constant.PoolMaxSize) 
        {
            var pool = new ObjectPool<T>(
                createFunc: () =>
                {
                    var obj = Instantiate(prefab);
                    obj.gameObject.SetActive(false);
                    obj.name = key;
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

            pools.Add(key, pool);
            return pool;
        }


        #endregion
        #region >--------------------------------------------------- GET


        protected override ObjectPool<T> GetPool<T>(T prefab, int defaultCapacity = Constant.PoolDefaultCapacity, int maxSize = Constant.PoolMaxSize)
        {
            var key = prefab.name;

            return pools.ContainsKey(key)
                ? pools[key] as ObjectPool<T>
                : CreatePool(key, prefab, defaultCapacity, maxSize);
        }

        public override T Get<T>(T prefab)
        {
            var key = prefab.name;
            var pool = GetPool(prefab);
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

            if (pools.TryGetValue(key, out var poolObj))
            {
                var pool = poolObj as ObjectPool<T>;
                pool.Release(obj);
            }

            if (activeObjects.ContainsKey(key))
            {
                activeObjects[key].Remove(obj);
            }
        }
        
        public override void ReleaseActiveObjects<T>(T prefab)
        {
            var key = prefab.name;

            var objects = new List<MonoBehaviour>(activeObjects[key]);

            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    Release(obj as T);
                }
            }

            activeObjects.Remove(key);
        }
        
        public override void ReleaseAllActiveObjects()
        {
            foreach (var activeSet in activeObjects.Values)
            {
                foreach (var obj in activeSet)
                {
                    if (obj != null)
                    {
                    }
                }

                activeSet.Clear();
            }

            activeObjects.Clear();
        }
   

        #endregion
        #region >--------------------------------------------------- DESTROY

        
        public override void DestroyActiveObjects<T>(T prefab)
        {
            var key = prefab.name;

            foreach (var obj in activeObjects[key])
            {
                if (obj != null)
                {
                    Destroy(obj.gameObject);
                }
            }

            activeObjects.Remove(key);
        }

        public override void DestroyAllActiveObjects() 
        {
            foreach (var activeSet in activeObjects.Values)
            {
                foreach (var obj in activeSet)
                {
                    if (obj != null)
                    {
                        Destroy(obj.gameObject);
                    }
                }

                activeSet.Clear();
            }

            activeObjects.Clear();
        }


        #endregion
    }
}