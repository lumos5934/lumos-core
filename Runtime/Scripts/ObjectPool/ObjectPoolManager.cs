using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Lumos.DevPack
{
    public class ObjectPoolManager : SingletonGlobal<ObjectPoolManager>, IBootable
    {
        #region >--------------------------------------------------- PROPERTIES

        public int Order => (int)BootsOrder.ObjectPool;
        public bool IsInitialized { get; private set; }


        #endregion
        #region >--------------------------------------------------- FIELDS


        private Dictionary<string, object> _pools = new();
        private Dictionary<string, HashSet<Component>> _activeObjects = new();

        
        #endregion
        #region >--------------------------------------------------- INIT


        public Task InitAsync()
        {
            IsInitialized = true;
            
            return Task.CompletedTask; 
        }


        #endregion
        #region >--------------------------------------------------- CREATE


        private ObjectPool<T> CreatePool<T>(string key, T prefab, int defaultCapacity = Constant.POOL_DEFAULT_CAPACITY,
            int maxSize = Constant.POOL_MAX_SIZE) where T : Component, IPoolable
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
                    obj.OnRealease();
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

            _pools.Add(key, pool);
            return pool;
        }


        #endregion
        #region >--------------------------------------------------- GET


        public ObjectPool<T> GetPool<T>(T prefab, int defaultCapacity = Constant.POOL_DEFAULT_CAPACITY,
            int maxSize = Constant.POOL_MAX_SIZE) where T : Component, IPoolable
        {
            var key = prefab.gameObject.name;

            return _pools.ContainsKey(key)
                ? _pools[key] as ObjectPool<T>
                : CreatePool(key, prefab, defaultCapacity, maxSize);
        }

        public T Get<T>(T prefab) where T : Component, IPoolable
        {
            var key = prefab.gameObject.name;
            var pool = GetPool(prefab);
            var obj = pool.Get();

            if (!_activeObjects.ContainsKey(key))
            {
                _activeObjects[key] = new HashSet<Component>();
            }

            _activeObjects[key].Add(obj);

            return obj;
        }


        #endregion
        #region >--------------------------------------------------- REALEASE


        public void Release<T>(T obj) where T : Component, IPoolable
        {
            var key = obj.gameObject.name;

            if (_pools.TryGetValue(key, out var poolObj))
            {
                var pool = poolObj as ObjectPool<T>;
                pool.Release(obj);
            }

            if (_activeObjects.ContainsKey(key))
            {
                _activeObjects[key].Remove(obj);
            }
        }


        #endregion
        #region >--------------------------------------------------- DESTROY


        public void DestroyActiveObjectsAll()
        {
            foreach (var activeSet in _activeObjects.Values)
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

            _activeObjects.Clear();
        }

        public void DestroyActiveObjects<T>(T prefab) where T : Component, IPoolable
        {
            var key = prefab.gameObject.name;

            foreach (var obj in _activeObjects[key])
            {
                if (obj != null)
                {
                    Destroy(obj.gameObject);
                }
            }

            _activeObjects.Remove(key);
        }


        #endregion
    }
}