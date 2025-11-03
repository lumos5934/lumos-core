using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LumosLib.Core
{
    public static class Global
    {
        #region >--------------------------------------------------- FIELDS

        
        private static readonly Dictionary<Type, IGlobal> Services = new();
        
        
        #endregion
        #region >--------------------------------------------------- REGISTER

        
        public static void Register<T>(T service) where T : IGlobal
        {
            if (Services.ContainsKey(typeof(T)))
            {
                Unregister<T>();
            }
         
            Services[typeof(T)] = service;
            
            if(service is MonoBehaviour serviceMono)
            {
                Object.DontDestroyOnLoad(serviceMono.gameObject);
            }
        }

        public static void Unregister<T>(T service) where T : IGlobal
        {
            Unregister<T>();
        }

        public static void Unregister<T>() where T : IGlobal
        {
            var service = Get<T>();
            if (service == null) return;
            
            Services.Remove(typeof(T));
            
            if(service is MonoBehaviour serviceMono)
            {
                Object.Destroy(serviceMono.gameObject);
            }
        }


        #endregion
        #region >--------------------------------------------------- GET

        
        public static T Get<T>() where T : IGlobal
        {
            if (Services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            DebugUtil.LogWarning($"{typeof(T)}", " NOT REGISTERED ");
            return default;
        }


        #endregion
    }
}