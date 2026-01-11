using UnityEngine;

namespace Lumos.Core
{
    public interface IResourceManager
    {
        public T Load<T>(string path) where T : Object;
        public T[] LoadAll<T>(string path) where T : Object;
    }
}