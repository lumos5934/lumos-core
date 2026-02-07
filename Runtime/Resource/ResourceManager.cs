using System.Collections.Generic;
using System.IO;
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
        
        
        #endregion
        #region  >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            foreach (var group in _groups)
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
            foreach (var group in _groups)
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
            
            
            foreach (var group in _groups)
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
            
            foreach (var group in _groups)
            {
                if (label != string.Empty)
                {
                    if(group.Label != label)
                        continue;
                }

                result.AddRange(group.GetResourcesAll<T>());
            }
            
            return result;
        }

        
        #endregion
        #region >--------------------------------------------------- INSPECTOR
        
        
        [Button("Collect All Resources")]
        public void SetEntriesResources()
        {
            List<string> usedPath = new();
            List<ResourceGroup> duplicateGroups = new();
            
            foreach (var group in _groups)
            {
                if (usedPath.Contains(group.Path))
                {
                    duplicateGroups.Add(group);
                    continue;
                }

                usedPath.Add(group.Path);
                group.SetResources(ResourcesUtil.Find<Object>(this, group.Path, SearchOption.TopDirectoryOnly));
            }

            foreach (var group in duplicateGroups)
            {
               _groups.Remove(group);
            }
        }
        
        
        #endregion
    }
}