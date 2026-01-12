using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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
            DestroyOldMonoService<T>();

            if (service is MonoBehaviour monoService)
            {
                Object.DontDestroyOnLoad(monoService.gameObject);
            }
            
            _services[typeof(T)] = service;
        }

        public static void Unregister<T>() where T : class
        {
            DestroyOldMonoService<T>();
            
            _services.Remove(typeof(T));
        }
        

        #endregion
        #region >--------------------------------------------------- GET

        
        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            DebugUtil.LogWarning($"{typeof(T)}", " NOT REGISTERED ");
            return null;
        }
        
        
        #endregion
        #region >--------------------------------------------------- DESTROY


        private static void DestroyOldMonoService<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var oldService))
            {
                if (oldService is MonoBehaviour monoBehaviour)
                {
                    UnityEngine.Object.Destroy(monoBehaviour.gameObject);
                }
            }
        }
        
        
        #endregion
    }
}