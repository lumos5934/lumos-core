using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LumosLib
{
    public static class Project
    {
        #region >--------------------------------------------------- PROPERTIE

        
        public static bool Initialized { get; private set; }
        public static ProjectConfig Config { get; private set; }
        
        public static float ElementInitElapsedMS => (float)((Time.realtimeSinceStartup - _elementInitStartMS) * 1000f);

        public static float InitProgress => (float)_curInitCount / _maxInitCount;
        private static string ElementInitText => $"( {_curInitCount}/{_maxInitCount} ) ( {ElementInitElapsedMS:F3} ms ) ";

    
        #endregion
        #region >--------------------------------------------------- FIELD


        private static double _elementInitStartMS;
        private static List<GameObject> _preloadObjects = new();
        
        private static int _curInitCount;
        private static int _maxInitCount;
        
        private static bool _isStartedInitAsync;
        

        #endregion
        #region >--------------------------------------------------- INIT
       

        public static IEnumerator InitAsync()
        {
            if(_isStartedInitAsync) yield break;
            
            _isStartedInitAsync = true;
            
            DebugUtil.Log($"", " INIT : START ");
            
                
            Config = Resources.Load<ProjectConfig>(Constant.ProjectConfig);
            if (Config == null)
            {
                PrintInitFail("not found ProjectConfig");
                yield break;
            }
            
            _elementInitStartMS = Time.realtimeSinceStartup;

            PreloadInternalInstance();
            
            for (int i = 0; i < Config.PreloadObjects.Count; i++)
            {
                var target = Config.PreloadObjects[i];
                
                if(target == null) continue;
                
                _preloadObjects.Add( Object.Instantiate(Config.PreloadObjects[i]).gameObject);
            }

            var preInitializes = new List<IPreInitializer>();
            var allActiveMono = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            for (int i = 0; i < allActiveMono.Length; i++)
            {
                if (allActiveMono[i] is IPreInitializer preInitialize)
                {
                    preInitializes.Add(preInitialize);
                }
            }

            preInitializes = preInitializes.OrderBy(x => x.PreInitOrder).ToList();

            _maxInitCount = preInitializes.Count + 1;
            _curInitCount++;
            
            PrintInitComplete(" Preload ");
            
            //Initialize
            for (int i = 0; i < preInitializes.Count; i++)
            {
                var target = preInitializes[i];
                
                _elementInitStartMS = Time.realtimeSinceStartup;
                
                yield return target.InitAsync();

                _curInitCount++;
                
                PrintInitComplete($" {target.GetType().Name} ");
            }

            
            DebugUtil.Log($"", " INIT : FINISH ");
            
            Initialized = true;
        }
        
        
        #endregion
        #region >--------------------------------------------------- PRINT

        
        public static void PrintInitComplete(string contents)
        {
            DebugUtil.Log($" {contents} {ElementInitText}", " INIT : COMPLETE ");
        }
        
        public static void PrintInitFail(string contents)
        {
            DebugUtil.LogError($" {contents} {ElementInitText}", " INIT : FAIL ");
        }

        
        #endregion
        #region >--------------------------------------------------- Preload

        private static void PreloadInternalInstance()
        {
            //MEMO : create internal object or class
            _ = new EventBus();
            CreateInternalResource<DataManager>();
            CreateInternalResource<ResourceManager>();
            CreateInternalResource<PoolManager>();
            CreateInternalResource<AudioManager>();
            CreateInternalResource<UIManager>();
            CreateInternalResource<PointerManager>();
        }
        
        private static void CreateInternalResource<T>() where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(typeof(T).Name);
            _preloadObjects.Add(Object.Instantiate(prefab).gameObject);
        }
        
        
        #endregion  
    }
}