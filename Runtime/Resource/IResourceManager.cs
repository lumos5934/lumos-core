using System.Collections.Generic;
using UnityEngine;

namespace LumosLib
{
    public interface IResourceManager
    {
        public T Get<T>(string path) where T : Object;
        public T Get<T>(string label, string path) where T : Object;
        public List<T> GetAll<T>(string label) where T : Object;
    }
}