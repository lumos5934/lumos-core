using System.Collections;
using UnityEngine;

namespace LumosLib
{
    public class ResourceManager : BaseResourceManager, IResourceManager
    {
        #region  >--------------------------------------------------- INIT
        
        
        public override IEnumerator InitAsync()
        {
            yield return base.InitAsync();
        }
        
        
        #endregion
        #region  >--------------------------------------------------- LOAD
       

        public override T Load<T>(string path)
        {
            if (cahcedResources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T;
            }

            return Resources.Load<T>(path);
        }

        public override T[] LoadAll<T>(string path)
        {
            if (cahcedResources.TryGetValue(path, out var cacheResource))
            {
                return cacheResource as T[];
            }

            return Resources.LoadAll<T>(path);
        }

        
        #endregion
    }
}