using UnityEngine;

namespace LumosLib.Core
{
    public interface IResourceManager : IGlobal
    {
        public T Load<T>(string path) where T : Object;
        public T[] LoadAll<T>(string path) where T : Object;
    }
}