using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib.Core
{
    public interface IPoolManager : IGlobal
    {
        public ObjectPool<T> GetPool<T>(T prefab, int defaultCapacity, int maxSize) where T : MonoBehaviour, IPoolable;
        public T Get<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public void Release<T>(T obj) where T : MonoBehaviour, IPoolable;
        public void DestroyActiveObjectsAll();
        public void DestroyActiveObjects<T>(T prefab) where T : MonoBehaviour, IPoolable;
    }
}