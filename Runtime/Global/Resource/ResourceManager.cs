using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public class ResourceManager : MonoBehaviour, IPreInitializer
    {
        #region  >--------------------------------------------------- PROPERTIES
        
        public int Order => (int)PreInitializeOrder.Resource;
        public bool IsInitialized { get; private set; }
        
        #endregion
        #region  >--------------------------------------------------- FIELDS


        private Dictionary<string, object> _resources = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT


        public Task InitAsync()
        {
            Global.Register(this);
            
            
            IsInitialized = true;
            return Task.CompletedTask;
        }
        
        
        #endregion
        #region  >--------------------------------------------------- LOAD


        public T Load<T>(string path) where T : Object
        {
            if (_resources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T;
            }

            var resource = Resources.Load<T>(path);
            if (resource != null)
            {
                _resources[path] = resource;
            }
            return resource;
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            if (_resources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T[];
            }

            var resource = Resources.LoadAll<T>(path);
            if (resource != null)
            {
                _resources[path] = resource;
            }
            return resource;
        }
        

        #endregion
    }
}