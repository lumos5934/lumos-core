using System.Collections.Generic;
using UnityEngine;

namespace Lumos.DevPack
{
    public static class Global
    {
        #region >--------------------------------------------------- FIELDS

        
        private static readonly Dictionary<System.Type, object> Services = new();
        
        
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
            
            if(service is Object serviceObj)
            {
                Object.DontDestroyOnLoad(serviceObj);
            }
        }

        public static void Unregister<T>() where T : class
        {
            var service = Get<T>();
            if (service == null) return;
            
            Services.Remove(typeof(T));
            
            if(service is Object serviceObj)
            {
                Object.Destroy(serviceObj);
            }
        }
        
        public static void Unregister<T>(T service) where T : class
        {
            Unregister<T>();
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