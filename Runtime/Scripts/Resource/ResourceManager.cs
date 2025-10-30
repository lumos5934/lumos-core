using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public class ResourceManager : SingletonGlobal<ResourceManager>, IBootable
    {
        #region  >--------------------------------------------------- PROPERTIES
        
        public int Order => (int)BootsOrder.Resource;
        public bool IsInitialized { get; private set; }
        
        #endregion
        #region  >--------------------------------------------------- FIELDS


        
        
        #endregion
        #region  >--------------------------------------------------- INIT


        public Task InitAsync()
        {
            IsInitialized = true;
            
            return Task.CompletedTask;
        }
        
        
        #endregion
        #region  >--------------------------------------------------- LOAD


    
        

        #endregion
    }
}