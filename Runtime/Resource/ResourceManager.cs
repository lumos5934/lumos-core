using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LumosLib
{
    public class ResourceManager : MonoBehaviour, IPreInitializable, IResourceManager
    {
        #region  >--------------------------------------------------- FIELD

        
        [SerializeField, 
         TableList(Draggable = true,
             HideAddButton = false,
             HideRemoveButton = false,
             AlwaysExpanded = false)] 
        private List<ResourceGroup> _groups;
        
        private readonly Dictionary<string, Object> _allResources = new();
        private Dictionary<string, List<ResourceGroup>> _allGroups = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            _allResources.Clear();
            _allGroups.Clear();

            foreach (var group in _groups)
            {
                if (string.IsNullOrEmpty(group.Label) || 
                    string.IsNullOrEmpty(group.FolderPath))
                    continue;

                if (!_allGroups.ContainsKey(group.Label))
                {
                    _allGroups[group.Label] = new List<ResourceGroup>();
                }
                
                _allGroups[group.Label].Add(group);

                var resources = Resources.LoadAll<Object>(group.FolderPath);
                group.SetResources(resources);

                foreach (var resource in resources)
                {
                    if (!_allResources.TryAdd(resource.name, resource))
                    {
                        DebugUtil.LogWarning($"fail add all resources: {resource.name} (label : {group.Label}, path: {group.FolderPath})", "Duplicate Name");
                    }
                }
            }

            GlobalService.Register<IResourceManager>(this);
            return UniTask.FromResult(true);
        }
    
        
        #endregion
        #region  >--------------------------------------------------- GET
       

        public T Get<T>(string assetName) where T : Object 
            => _allResources.GetValueOrDefault(assetName) as T;

        public T Get<T>(string label, string assetName) where T : Object
        {
            if (string.IsNullOrEmpty(label)) return Get<T>(assetName);

            if (_allGroups.TryGetValue(label, out var groups))
            {
                foreach (var group in groups)
                {
                    var asset = group.GetResource<T>(assetName);
                    if (asset != null) return asset;
                }
            }
            return null;
        }

        public List<T> GetAll<T>(string label) where T : Object
        {
            if (string.IsNullOrEmpty(label)) return GetAll<T>();

            if (_allGroups.TryGetValue(label, out var groups))
            {
                return groups.SelectMany(g => g.GetResourcesAll<T>()).Distinct().ToList();
            }
            
            return new List<T>();
        }

        public List<T> GetAll<T>() where T : Object 
            => _allResources.Values.OfType<T>().ToList();

        
        #endregion
    }
}