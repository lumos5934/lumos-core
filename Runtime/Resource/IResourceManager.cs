using System.Collections.Generic;
using UnityEngine;

namespace LumosLib
{
    public interface IResourceManager
    {
        public T Get<T>(string assetName) where T : Object;
        public T Get<T>(string label, string key) where T : Object;
        public List<T> GetAll<T>() where T : Object;
        public List<T> GetAll<T>(string label) where T : Object;
    }
}