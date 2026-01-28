using System;
using System.Collections.Generic;
using System.IO;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    [Serializable]
    public class ResourceEntryGroup
    {
        public string Label => _label;
        public string FolderPath => _folderPath;

        [SerializeField, LabelWidth(5)] private string _folderPath;
        [SerializeField] private string _label;
        [SerializeField, ReadOnly] private List<ResourceEntry> _entries;

        private Dictionary<string, ResourceEntry> _entriesDict;

        public void Init()
        {
            _entriesDict = new();
            
            foreach (var entry in _entries)
            {
                _entriesDict[entry.key] = entry;
            }
        }
        
        public T GetResource<T>(string entryName)
        {
            if (_entriesDict.TryGetValue(entryName, out var entry))
            {
                return entry.GetResource<T>();
            }
            
            return default;
        }

        public List<T> GetResourcesAll<T>()
        {
            var result = new List<T>();

            foreach (var entry in _entries)
            {
                var resource = entry.GetResource<T>();

                if (resource != null)
                {
                    result.Add(resource);
                }
            }

            return result;
        }

        public void SetResources(List<ResourceEntry> entries)
        {
            _entries = entries;
        }
    }
}