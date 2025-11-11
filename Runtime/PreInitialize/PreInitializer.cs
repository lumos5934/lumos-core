using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LumosLib
{
    public class PreInitializer : SingletonGlobal<PreInitializer>
    {
        public bool Initialized { get; private set; }
        public PreInitializeConfigSO Config { get; private set; }
        
        private List<IPreInitialize> PreInitializes = new();
        
        
        protected override void Awake()
        {
            base.Awake();
            
            Config = Resources.Load<PreInitializeConfigSO>(Constant.PathPreInitializerConfig);
            if (Config == null)
            {
                DebugUtil.LogWarning($" not found Reousrces/{Constant.PathPreInitializerConfig} "," INIT FAIL ");
                return;
            }
            
            foreach (var mono in Config.PreInitializes)
            {
                if (mono is IPreInitialize preInit)
                {
                    PreInitializes.Add(preInit);
                }
            }
            
            var idHash = new HashSet<int>();
            PreInitializes.RemoveAll(x => !idHash.Add(x.PreInitOrder));
            PreInitializes = PreInitializes.OrderBy(x => x.PreInitOrder).ToList();
            
            StartCoroutine(InitAsync(PreInitializes));
        }
        
        
        private IEnumerator InitAsync(List<IPreInitialize> preInitializes)
        {
            var startTime = Time.realtimeSinceStartup;
            DebugUtil.Log("", " INITIALIZED - START ");
            
            
            //Initialize
            for (int i = 0; i < preInitializes.Count; i++)
            {
                var initStartTime = Time.realtimeSinceStartup;
                
                var prefab = (preInitializes[i] as MonoBehaviour).gameObject;
                
                var instance = Instantiate(prefab);
                DontDestroyOnLoad(instance);
                
                var initialize = instance.GetComponent<IPreInitialize>();
                
                if (!initialize.PreInitialized)
                {
                    yield return new WaitUntil(() => initialize.PreInitialized); 
                }

                DebugUtil.Log($" { initialize.GetType().Name } ( {(Time.realtimeSinceStartup - initStartTime) * 1000f:F3} ms )", $" INITIALIZED ");
            }


            var totalElapsed = Time.realtimeSinceStartup - startTime;
            DebugUtil.Log($" ( {totalElapsed * 1000f:F3} ms )", " INITIALIZED - FINISHED ");


            Initialized = true;
         
            Destroy(gameObject);
        }
    }
}