using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LumosLib
{
    public class ResourceManager : MonoBehaviour, IPreInitializable, IResourceManager
    {
        #region  >--------------------------------------------------- FIELD


        private Dictionary<string, object> _cahcedResources = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT
        
        
        public IEnumerator InitAsync(Action<bool> onComplete)
        {
            GlobalService.Register<IResourceManager>(this);
            DontDestroyOnLoad(gameObject);
            
            onComplete?.Invoke(true);
            
            yield break;
        }
        
        
        #endregion
        #region  >--------------------------------------------------- LOAD
       

        public T Load<T>(string path) where T : Object
        {
            if (_cahcedResources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T;
            }
            
            var resoruce = Resources.Load<T>(path);
            if (resoruce != null)
            {
                _cahcedResources.Add(path, resoruce);
            }

            return resoruce;
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            if (_cahcedResources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T[];
            }

            var resources = Resources.LoadAll<T>(path);
            if (resources != null && resources.Length > 0)
            {
                _cahcedResources.Add(path, resources);
            }
            
            return resources;
        }

        
        #endregion
    }
}