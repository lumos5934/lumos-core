using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace LLib.Core
{
    public class PoolManager : BasePoolManager
    {
        #region >--------------------------------------------------- PROPERTIES

        public override int PreInitOrder => (int)PreInitializeOrder.Pool;


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


        public override ObjectPool<T> GetPool<T>(T prefab, int defaultCapacity = Constant.PoolDefaultCapacity, int maxSize = Constant.PoolMaxSize)
        {
            var key = prefab.gameObject.name;

            return pools.ContainsKey(key)
                ? pools[key] as ObjectPool<T>
                : CreatePool(key, prefab, defaultCapacity, maxSize);
        }

        public override T Get<T>(T prefab)
        {
            var key = prefab.gameObject.name;
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
            var key = obj.gameObject.name;

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


        #endregion
        #region >--------------------------------------------------- DESTROY


        public override void DestroyActiveObjectsAll()
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

        public override void DestroyActiveObjects<T>(T prefab)
        {
            var key = prefab.gameObject.name;

            foreach (var obj in activeObjects[key])
            {
                if (obj != null)
                {
                    Destroy(obj.gameObject);
                }
            }

            activeObjects.Remove(key);
        }


        #endregion
    }
}