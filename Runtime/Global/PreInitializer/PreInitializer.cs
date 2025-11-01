using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public static class PreInitializer
    {
        #region >--------------------------------------------------- PROPERTIES


        public static bool IsInitialized => _isInitialized;
        
        
        #endregion
        #region >--------------------------------------------------- FIELDS

        
        private static bool _isInitialized = false;
        
        #endregion
        #region >--------------------------------------------------- INIT


        static PreInitializer()
        {
            _ = InitAsync();
        }
        
        private static async Task InitAsync()
        {
            if (_isInitialized) return;
            
            var initializerPrefabs 
                = Resources.LoadAll<MonoBehaviour>(Path.PreInitializer)
                    .OfType<IPreInitializer>()
                        .OrderBy(x => x.Order); 
            
            
            var sortedInitializer = initializerPrefabs.OrderBy(x => x.Order).ToList();

            foreach (var initializer in sortedInitializer)
            {
                var prefab = initializer as MonoBehaviour;
                var instance = Object.Instantiate(prefab);
                var initializerInstance = instance as IPreInitializer;

                instance.gameObject.name = prefab.name;
                
                await initializerInstance.InitAsync();

                DebugUtil.Log(" INIT COMPLETE ", $" { instance.name } ");
            }

            _isInitialized = true;
            DebugUtil.Log("", " All INIT COMPLETE ");
        }
        
        
        #endregion
    }
}