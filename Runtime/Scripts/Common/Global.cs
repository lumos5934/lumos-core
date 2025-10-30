using System;
using System.Collections.Generic;

namespace Lumos.DevPack
{
    public static class Global
    {
        #region >--------------------------------------------------- FIELDS

        
        private static readonly Dictionary<Type, object> Services = new();

        
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