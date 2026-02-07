using System;
using System.Collections.Generic;
using System.IO;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    [Serializable]
    public class ResourceGroup
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

        public void SetResources(List<ResourceElement> entries)
        {
            _elements = entries;
        }
    }
}