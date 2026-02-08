using System;
using System.Collections.Generic;
using System.IO;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LumosLib
{
    [Serializable]
    public class ResourceElementGroup
    {
        public string Label => _label;
        public string Path => _path;

        [Group("Address"), SerializeField, LabelWidth(50)] private string _path;
        [Group("Address"), SerializeField, LabelWidth(50)] private string _label;
        [Group("Element"), SerializeField, ReadOnly, LabelText("Key")] private List<ResourceElement> _elements;

        private Dictionary<string, ResourceElement> _elementsDict;
    
        public void Init()
        {
            _elementsDict = new();
            
            foreach (var entry in _elements)
            {
                _elementsDict[entry.key] = entry;
            }
        }
        
        public T GetResource<T>(string entryName)
        {
            if (_elementsDict.TryGetValue(entryName, out var entry))
            {
                return entry.GetResource<T>();
            }
            
            return default;
        }

        public List<T> GetResourcesAll<T>()
        {
            var result = new List<T>();

            foreach (var entry in _elements)
            {
                var resource = entry.GetResource<T>();

                if (resource != null)
                {
                    result.Add(resource);
                }
            }

            return result;
        }

        public void SetResources(Dictionary<string, List<Object>> resources)
        {
            _elements = new();

            foreach (var resourceKvp in resources)
            {
                var newElement =  new ResourceElement();
                newElement.key = resourceKvp.Key;
                newElement.resources =  resourceKvp.Value;
                
                _elements.Add(newElement);
            }
        }
    }
}