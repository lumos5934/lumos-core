using System.Collections;
using UnityEngine;

namespace LLib.Core
{
    public abstract class BaseSceneManager : SingletonScene<BaseSceneManager>, IGlobal
    {
        #region --------------------------------------------------- UNITY


        protected override void Awake()
        {
            base.Awake();
            
            StartCoroutine(InitAsync());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Global.Unregister(this);
        }


        #endregion
        #region --------------------------------------------------- INIT


        protected virtual void Init()
        {
            Global.Register(this);
        }
        
        private IEnumerator InitAsync() 
        {
            yield return new WaitUntil(() => PreInitializer.Initialized);

            Init();
        }


        #endregion
    }
}