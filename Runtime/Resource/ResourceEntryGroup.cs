using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    [Serializable, 
     DeclareHorizontalGroup("horizontal")]
    public class ResourceEntryGroup
    {
        public bool UseLabel => _useLabel;
        public string Label => _label;
        public string FolderPath => _folderPath;

        [SerializeField, Group("horizontal"), LabelText("Label")] private bool _useLabel;
        [SerializeField, Group("horizontal"), ShowIf("_useLabel"), HideLabel] private string _label;
        [SerializeField] private string _folderPath;
        [SerializeField, 
         ReadOnly, 
         TableList(Draggable = true,
            HideAddButton = false,
            HideRemoveButton = false,
            AlwaysExpanded = false)] private List<ResourceEntry> _entries;

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