using UnityEngine;
using UnityEngine.Pool;

namespace LumosLib
{
    public interface IPoolManager
    {
        public T Get<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public void Release<T>(T obj) where T : MonoBehaviour, IPoolable;
        public void ReleaseAll<T>(T prefab) where T : MonoBehaviour, IPoolable;
        public void DestroyAll<T>(T prefab) where T : MonoBehaviour, IPoolable;
    }
}