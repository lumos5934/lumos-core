using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace LumosLib
{
    public static class PreInitializer
    {
        #region >--------------------------------------------------- PROPERTIES

        
        public static bool IsInitialized => _isInitialized;

        public static float InitElapsedMS =>
            (float)((Time.realtimeSinceStartup - _elementInitStartTime) * 1000f);

        public static float InitProgress =>
            _maxInitCount == 0 ? 1f : (float)_curInitCount / _maxInitCount;

        
        #endregion
        #region >--------------------------------------------------- FIELDS

        
        private static double _elementInitStartTime;

        private static int _curInitCount;
        private static int _maxInitCount;
        private static int _failCount;

        private static bool _isInitializing;
        private static bool _isInitialized;

        private static UniTaskCompletionSource _initBarrier;
        private static LumosLibSettings _settings;
        

        #endregion
        #region >--------------------------------------------------- INIT

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Boot()
        {
            _initBarrier = new UniTaskCompletionSource();
            
            _settings = Resources.Load<LumosLibSettings>(nameof(LumosLibSettings));
            
            if (_isInitializing || _isInitialized || _settings == null)
                return;

            if (!_settings.UsePreload)
                return;


            _isInitializing = true;

            Initialize().Forget();
        }
        
        public static UniTask WaitInitAsync()
        {
            if (_isInitialized)
                return UniTask.CompletedTask;

            return _initBarrier.Task;
        }

        private static async UniTask Initialize()
        {
            DebugUtil.Log("", " INIT : START ");

            _elementInitStartTime = Time.realtimeSinceStartup;

            var initTargets = new List<IPreInitializable>();

            foreach (var prefab in _settings.PreloadObjects)
            {
                if (prefab == null)
                    continue;

                var obj = Object.Instantiate(prefab);

                if (obj.TryGetComponent(out IPreInitializable initializer))
                {
                    initTargets.Add(initializer);
                }
            }

            _maxInitCount = initTargets.Count;

            foreach (var target in initTargets)
            {
                _elementInitStartTime = Time.realtimeSinceStartup;

                bool success = false;
                try
                {
                    success = await target.InitAsync();
                }
                catch (System.Exception e)
                {
                    DebugUtil.LogError(e.ToString(), " INIT : EXCEPTION ");
                }

                _curInitCount++;

                if (success)
                {
                    DebugUtil.Log(
                        $"{target.GetType().Name}",
                        $" INIT : SUCCESS ({_curInitCount}/{_maxInitCount})"
                    );
                }
                else
                {
                    _failCount++;
                    DebugUtil.LogError(
                        $"{target.GetType().Name}",
                        $" INIT : FAIL ({_curInitCount}/{_maxInitCount})"
                    );
                }
            }

            DebugUtil.Log(
                "",
                $" INIT : FINISH - COMPLETE : {_curInitCount - _failCount}, FAIL : {_failCount}"
            );

            FinishInit();
        }
        
        private static void FinishInit()
        {
            _isInitialized = true;
            _isInitializing = false;

            _initBarrier.TrySetResult();
        }
        
        #endregion
    }
}