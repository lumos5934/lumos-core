using System;
using System.Collections.Generic;

namespace LumosLib
{
    public static class GlobalService
    {
        #region >--------------------------------------------------- FIELD

        
        private static Dictionary<Type, object> _services = new();
        
        
        #endregion
        #region >--------------------------------------------------- REGISTER

        
        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public static void Unregister<T>() where T : class
        {
            _services.Remove(typeof(T));
        }
        

        #endregion
        #region >--------------------------------------------------- GET

        //for project
        public static T Get<T>() where T : class
        {
            return GetService<T>();
        }
        
        //for package
        internal static T GetInternal<T>() where T : class
        {
            return GetService<T>();
        }

        private static T GetService<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            DebugUtil.LogWarning($"{typeof(T)}", " NOT REGISTERED ");
            return null;
        }
        
        
        #endregion
    }
}