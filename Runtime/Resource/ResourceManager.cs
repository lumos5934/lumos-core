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
         LabelText("Entries"), 
         TableList(Draggable = true,
             HideAddButton = false,
             HideRemoveButton = false,
             AlwaysExpanded = false)] 
        private List<ResourceEntryGroup> _entryGroups;
        
        
        
        #endregion
        #region  >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            foreach (var group in _entryGroups)
            {
                group.Init();
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
            if(label == "") 
                return Get<T>(assetName);
            
            
            foreach (var group in _entryGroups)
            {
                if(group.Label != label) 
                    continue;

                return Get<T>(assetName);
            }
            
            return default;
        }

        public List<T> GetAll<T>(string label)
        {
            var result = new List<T>();
            
            foreach (var group in _entryGroups)
            {
                if(group.Label != label)
                    continue;

                result.AddRange(group.GetResourcesAll<T>());
            }
            
            return result;
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
        }
        
        
        #endregion
    }
}