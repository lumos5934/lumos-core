using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib
{
    public interface IPoolManager
    {
        public T Get<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public void Release<T>(T obj) where T : MonoBehaviour, IPoolable;
        /*public void ReleaseActiveObjects<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public void DestroyActiveObjects<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public void DestroyAllActiveObjects();*/
    }
}