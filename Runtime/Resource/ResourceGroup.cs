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
        public Dictionary<string, Object> Resources => _resources;
        
        [Group("Folder Path"), 
         SerializeField, 
         HideLabel, 
         Required] private string _folderPath;
        
        [Group("Label "), 
         SerializeField, 
         HideLabel , 
         Required] private string _label;
        

        private readonly Dictionary<string, Object> _resources = new();

  
        public void SetResources(Object[] resources)
        {
            foreach (var resource in resources)
            {
                _resources.TryAdd(resource.name, resource);
            }
        }
    }
}