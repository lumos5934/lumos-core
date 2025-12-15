using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LumosLib
{
    public abstract class BaseResourceManager : MonoBehaviour, IPreInitializer, IResourceManager
    {
        #region  >--------------------------------------------------- FIELD


        protected Dictionary<string, object> cahcedResources = new();
        
        
        #endregion
        #region  >--------------------------------------------------- INIT


        public virtual IEnumerator InitAsync()
        {
            GlobalService.Register<IResourceManager>(this);
            DontDestroyOnLoad(gameObject);
            
            yield break;
        }


        #endregion
        #region  >--------------------------------------------------- LOAD

        public abstract T Load<T>(string path) where T : Object;
        public abstract T[] LoadAll<T>(string path) where T : Object;
        
        
        #endregion
    }
}