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

            var preInitializes = new List<IPreInitializer>();
            
            for (int i = 0; i < Config.PreloadObjects.Count; i++)
            {
                var preloadPrefab = Config.PreloadObjects[i];
                if(preloadPrefab == null) continue;

                var preloadObj = Object.Instantiate(Config.PreloadObjects[i]).gameObject;

                if (preloadObj.TryGetComponent(out IPreInitializer initializer))
                {
                    preInitializes.Add(initializer);
                }
            }
            
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
    }
}