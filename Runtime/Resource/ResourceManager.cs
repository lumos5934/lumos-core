using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LumosLib
{
    [DeclareBoxGroup("Resources", Title = "Resources")]
    public class ResourceManager : MonoBehaviour, IPreInitializable, IResourceManager
    {
        #region  >--------------------------------------------------- FIELD

        
        [Group("Resources"), SerializeField, LabelText("Entries")] private List<ResourceEntry> _resourceEntries;
        
        private Dictionary<string, Object> _allResources = new();
        private Dictionary<string, ResourceEntry> _resourceEntriesDict = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            foreach (var entry in _resourceEntries)
            {
                entry.Init();
                
                if (!_resourceEntriesDict.TryAdd(entry.Label, entry))
                {
                    DebugUtil.LogError("duplicate entry label", "Resource");
                    return UniTask.FromResult(false);
                }
                
                foreach (var resource in entry.Resources)
                {
                    if (!_allResources.TryAdd(resource.name, resource))
                    {
                        DebugUtil.LogError("duplicate resource name", "Resource");
                        return UniTask.FromResult(false);
                    }
                }
            }
            
            GlobalService.Register<IResourceManager>(this);
            return UniTask.FromResult(true);
        }
    
        
        #endregion
        #region  >--------------------------------------------------- LOAD
       

        public T Get<T>(string fileName) where T : Object
        {
            if (_allResources.TryGetValue(fileName, out var resource))
            {
                if (resource is T t)
                {
                    return t;
                }
            }

            return null;
        }
        
        public T Get<T>(string label, string fileName) where T : Object
        {
            if (_resourceEntriesDict.TryGetValue(label, out var entry))
            {
                var resource = entry.GetResource(fileName);
                if (resource is T t)
                {
                    return t;
                }
            }
            
            return null;
        }

        public List<T> GetAll<T>(string label) where T : Object
        {
            if (_resourceEntriesDict.TryGetValue(label, out var entry))
            {
                return entry.Resources
                    .OfType<T>()  
                    .ToList();
            }
            
            return null;
        }

        
        #endregion
        #region >--------------------------------------------------- INSPECTOR
        
        
        [Group("Resources"), Button("Collect All Resources")]
        public void SetEntriesResources()
        {
            foreach (var entry in _resourceEntries)
            {
                entry.SetResources(ResourcesUtil.Find<Object>(this, entry.FolderPath, SearchOption.TopDirectoryOnly));
            }
        }
        
        
        #endregion
    }
}