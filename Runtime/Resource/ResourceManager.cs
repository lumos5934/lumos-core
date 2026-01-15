using System.Collections.Generic;
using System.IO;
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

        
        [Group("Resources"), 
         SerializeField, 
         LabelText("Entries")] 
        private List<ResourceEntryGroup> _entryGroups;
        
        private Dictionary<(bool, string), ResourceEntryGroup> _entryGroupsDict = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            foreach (var group in _entryGroups)
            {
                group.Init();
                
                if (!_entryGroupsDict.TryAdd((group.UseLabel, group.Label), group))
                {
                    DebugUtil.LogError("duplicate entry label", "Resource");
                    return UniTask.FromResult(false);
                }
            }
            
            GlobalService.Register<IResourceManager>(this);
            return UniTask.FromResult(true);
        }
    
        
        #endregion
        #region  >--------------------------------------------------- GET
       

        public T Get<T>(string assetName)
        {
            foreach (var group in _entryGroups)
            {
                var result = group.GetResource<T>(assetName);

                if (result != null)
                {
                    return result;
                }
            }

            return default;
        }
        
        public T Get<T>(string label, string assetName)
        {
            if (_entryGroupsDict.TryGetValue((true, label), out var entry))
            {
                return entry.GetResource<T>(assetName);
            }
            
            return default;
        }

        public List<T> GetAll<T>(string label)
        {
            if (_entryGroupsDict.TryGetValue((true, label), out var entry))
            {
                return entry.GetResourcesAll<T>();
            }
            
            return default;
        }

        
        #endregion
        #region >--------------------------------------------------- INSPECTOR
        
        
        [Group("Resources"), Button("Collect All Resources")]
        public void SetEntriesResources()
        {
            foreach (var group in _entryGroups)
            {
                group.SetResources(ResourcesUtil.Find<Object>(this, group.FolderPath, SearchOption.TopDirectoryOnly));
            }
            
            /*foreach (var entry in _resourceEntries)
            {
                entry.SetResources(ResourcesUtil.FindObjects<Object>(this, entry.FolderPath, SearchOption.TopDirectoryOnly));
            }*/
        }
        
        
        #endregion
    }
}