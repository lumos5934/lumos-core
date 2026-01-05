using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace LumosLib
{
    public static class PreInitializer
    {
        #region >--------------------------------------------------- PROPERTIE

        public static LumosLibSetting Setting { get; private set; }
        
        public static float InitElapsedMS => (float)((Time.realtimeSinceStartup - _elementInitStartMS) * 1000f);
        public static float InitProgress => (float)_curInitCount / _maxInitCount;
        private static string InitResultText => $"( {_curInitCount}/{_maxInitCount} ) ( {InitElapsedMS:F3} ms ) ";

    
        #endregion
        #region >--------------------------------------------------- FIELD

        
        private static double _elementInitStartMS;
        
        private static int _curInitCount;
        private static int _maxInitCount;
        
        private static bool _isStartedInitAsync;
        
        private static int _failCount;
        
        private static UniTask _initTask;
        private static bool _isInitialized;

        
        #endregion
        #region >--------------------------------------------------- INIT

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static UniTask InitializeAsync()
        {
            Setting = Resources.Load<LumosLibSetting>(nameof(LumosLibSetting));
            if (_isInitialized || !Setting.UsePreload)
            {
                return UniTask.CompletedTask;
            }
            
            if (_initTask.Status == UniTaskStatus.Pending)
                return _initTask;

            
            _initTask = Initialize();
            return _initTask;
        }

        private static async UniTask Initialize()
        {
            DebugUtil.Log($"", " INIT : START ");
            
            _elementInitStartMS = Time.realtimeSinceStartup;
            
            var initTargets = new List<IPreInitializable>();
            
            for (int i = 0; i < Setting.PreloadObjects.Count; i++)
            {
                var preloadPrefab = Setting.PreloadObjects[i];
                if(preloadPrefab == null) continue;

                var preloadObj = Object.Instantiate(Setting.PreloadObjects[i]).gameObject;

                if (preloadObj.TryGetComponent(out IPreInitializable initializer))
                {
                    initTargets.Add(initializer);
                }
            }
            
            _maxInitCount = initTargets.Count;
            
            for (int i = 0; i < initTargets.Count; i++)
            {
                var target = initTargets[i];
                
                _elementInitStartMS = Time.realtimeSinceStartup;

                bool isSuccess = await target.InitAsync();
                
                _curInitCount++;

                if (isSuccess)
                {
                   DebugUtil.Log($" {target.GetType().Name} {InitResultText}", " INIT : SUCCESS ");
                }
                else
                {
                   DebugUtil.LogError($" {target.GetType().Name} {InitResultText}", " INIT : FAIL ");
                   
                   _failCount++;
                }
            }
            
            DebugUtil.Log($"", $" INIT : FINISH - COMPLETE : { _curInitCount - _failCount }, FAIL : { _failCount }");
            _isInitialized = true;
        }
        
        
        #endregion
    }
}