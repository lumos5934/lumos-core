using UnityEngine;

namespace LLib.Core
{
    public class ResourceManager : BaseResourceManager
    {
        #region  >--------------------------------------------------- PROPERTIES
        
        
        public override int PreInitOrder => (int)PreInitializeOrder.Resource;
        
        
        #endregion
        #region  >--------------------------------------------------- INIT


        public override void PreInit()
        {
            base.PreInit();
            
            PreInitialized = true;
        }
        
        
        #endregion
        #region  >--------------------------------------------------- LOAD


        protected override T GetResource<T>(string path)
        {
            var resource = Resources.Load<T>(path);
            if (resource != null)
            {
                cahcedResources[path] = resource;
            }
            
            return resource;
        }

        protected override T[] GetResourceAll<T>(string path)
        {
            var resource = Resources.LoadAll<T>(path);
            if (resource != null)
            {
                cahcedResources[path] = resource;
            }
            return resource;
        }
        

        #endregion

    }
}