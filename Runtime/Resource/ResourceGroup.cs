using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LumosLib
{
    [Serializable]
    public class ResourceGroup
    {
        public string FolderPath => _folderPath;
        public string Label => _label;
        
        [Group("Label "), 
         SerializeField, 
         HideLabel , 
         Required] private string _label;
        
        [Group("Folder Path"), 
         SerializeField, 
         HideLabel, 
         Required] private string _folderPath;

        private readonly Dictionary<string, Object> _resources = new();

        
        public T GetResource<T>(string entryName) where T : Object
        {
            if (_resources.TryGetValue(entryName, out var resource))
            {
                return resource as T;
            }
            
            return null;
        }

        public List<T> GetResourcesAll<T>() where T : Object
        {
            var result = new List<T>();

            foreach (var resource in _resources)
            {
                if (resource.Value is T t)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        public void SetResources(Object[] resources)
        {
            foreach (var resource in resources)
            {
                if (!_resources.TryAdd(resource.name, resource))
                {
                    DebugUtil.LogWarning($"fail add {_label} resources: {resource.name} (path: {_folderPath})", "Duplicate Name");
                }
            }
        }

    }
}