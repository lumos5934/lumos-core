using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public static class Global
    {
        #region >--------------------------------------------------- PROPERTIES


        public static bool IsInitialized => _isInitialized;
        
        
        #endregion
        #region >--------------------------------------------------- FIELDS

        
        private static readonly Dictionary<System.Type, object> Services = new();
        private static bool _isInitialized = false;
        
        #endregion
        #region >--------------------------------------------------- INIT


        static Global()
        {
            _ = InitAsync();
        }
        
        private static async Task InitAsync()
        {
            if (_isInitialized) return;
            
            var bootablePrefabs = Resources.LoadAll<MonoBehaviour>(Constant.BOOT)
                .OfType<IBootable>()
                .OrderBy(x => x.Order); 
            
            
            var sortedBootable = bootablePrefabs.OrderBy(x => x.Order).ToList();

            foreach (var bootable in sortedBootable)
            {
                var bootableObject = GameObject.Instantiate( bootable as MonoBehaviour).gameObject;
                
                await bootable.InitAsync();
                
                Register(bootable);
                GameObject.DontDestroyOnLoad(bootableObject);
                DebugUtil.Log(" INIT COMPLETE ", $" { bootableObject.name } ");
            }

            _isInitialized = true;
            DebugUtil.Log("", " All Managers INIT COMPLETE ");
        }
        
        
        #endregion
        #region >--------------------------------------------------- REGISTER

        
        public static void Register<T>(T service) where T : class
        {
            if (service == null)
            {
                DebugUtil.LogWarning($"{typeof(T)} - null", " FAIL REGISTER ");
                return;
            }

            Services[typeof(T)] = service;
        }

        public static void Unregister<T>() where T : class
        {
            Services.Remove(typeof(T));
        }
        
        public static void Unregister<T>(T service) where T : class
        {
            Services.Remove(service.GetType());
        }
        

        #endregion
        #region >--------------------------------------------------- GET

        
        public static T Get<T>() where T : class
        {
            if (Services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }

            DebugUtil.LogWarning($"{typeof(T)} - not registered", " FAIL GET ");
            return null;
        }

        public static bool GetExists<T>() where T : class
        {
            return Services.ContainsKey(typeof(T));
        }
        

        #endregion
    }
}