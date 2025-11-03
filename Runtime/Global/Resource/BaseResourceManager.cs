using System.Collections.Generic;
using UnityEngine;

namespace LumosLib.Core
{
    public abstract class BaseResourceManager : MonoBehaviour, IResourceManager, IPreInitialize
    {
        #region  >--------------------------------------------------- PROPERTIE

        
        public int PreID => (int)PreInitializeOrder.Resource;
        public abstract int PreInitOrder { get; }
        public bool PreInitialized { get; protected set; }
        
        
        #endregion
        #region  >--------------------------------------------------- FIELD


        protected Dictionary<string, object> cahcedResources = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT


        public virtual void PreInit()
        {
            Global.Register((IResourceManager)this);
        }
        
        
        #endregion
        #region  >--------------------------------------------------- LOAD


        public T Load<T>(string path) where T : Object
        {
            if (cahcedResources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T;
            }

            return GetResource<T>(path);
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            if (cahcedResources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T[];
            }
            
            return GetResourceAll<T>(path);
        }
      

        #endregion
        #region  >--------------------------------------------------- GET


        protected abstract T GetResource<T>(string path) where T : Object;
        protected abstract T[] GetResourceAll<T>(string path) where T : Object;
        
        
        #endregion
    }
}