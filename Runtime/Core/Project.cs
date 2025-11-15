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
            
            //Instantiate Preload Objects
            InstantiatePreloadObjects(Config);

            var preInitializes = new List<IPreInitialize>();
            var allActiveMono = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            for (int i = 0; i < allActiveMono.Length; i++)
            {
                if (allActiveMono[i] is IPreInitialize preInitialize)
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
        #region >--------------------------------------------------- INSTANTIATE
        
        
        private static void InstantiatePreloadObjects(ProjectConfig config)
        {
            //MEMO : Add LumosLib Object
            InstantiatePackageResource<DataManager>();
            InstantiatePackageResource<ResourceManager>();
            InstantiatePackageResource<PoolManager>();
            InstantiatePackageResource<AudioManager>();
            InstantiatePackageResource<UIManager>();
            //
         
            for (int i = 0; i < config.PreloadObjects.Count; i++)
            {
                _preloadObjects.Add( Object.Instantiate(config.PreloadObjects[i]).gameObject);
            }
        }
        
        private static void InstantiatePackageResource<T>() where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(typeof(T).Name);
            _preloadObjects.Add(Object.Instantiate(prefab).gameObject);
        }
        
        
        #endregion  
    }
}