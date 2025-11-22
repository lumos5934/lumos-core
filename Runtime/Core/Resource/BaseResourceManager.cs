using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LumosLib
{
    public abstract class BaseResourceManager : MonoBehaviour, IPreInitializer, IResourceManager
    {
        #region  >--------------------------------------------------- PROPERTIE

        
        public int PreInitOrder => (int)PreInitializeOrder.Resource;
      
        
        #endregion
        #region  >--------------------------------------------------- FIELD


        protected Dictionary<string, object> cahcedResources = new();
        
        
        #endregion
        #region  >--------------------------------------------------- UNITY


        public virtual void Awake()
        {
            GlobalService.Register<IResourceManager>(this);
            
            DontDestroyOnLoad(gameObject);
        }
        
        
        #endregion
        #region  >--------------------------------------------------- INIT


        public abstract IEnumerator InitAsync();


        #endregion
        #region  >--------------------------------------------------- LOAD

        public abstract T Load<T>(string path) where T : Object;
        public abstract T[] LoadAll<T>(string path) where T : Object;
        
        
        #endregion
    }
}