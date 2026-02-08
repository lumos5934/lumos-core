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

        [SerializeField] private string _rootPath;
        
        [SerializeField, 
         TableList(Draggable = true,
             HideAddButton = false,
             HideRemoveButton = false,
             AlwaysExpanded = false)] 
        private List<ResourceElementGroup> _groups;
        
        
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
        private void SetResourcesGroup()
        {
            var usedPath = new List<string>();
            var useGroup = new List<ResourceElementGroup>();
            
            foreach (var group in _groups)
            {
                if (usedPath.Contains(group.Path)) 
                    continue;
                
                useGroup.Add(group);
                usedPath.Add(group.Path);
            }

            foreach (var group in useGroup)
            {
                group.SetResources(AssetFinder.Find<Object>(this, _rootPath + "/" + group.Path, SearchOption.TopDirectoryOnly));
            }
            
            _groups = useGroup;
        }
        
        
        #endregion
    }
}