using System.Collections;
using UnityEngine;

namespace Lumos.DevPack
{
    public abstract class BaseSceneManager : SingletonScene<BaseSceneManager>
    {
        #region --------------------------------------------------- PROPERTIES




        #endregion
        #region --------------------------------------------------- UNITY


        protected override void Awake()
        {
            base.Awake();
            
            StartCoroutine(InitAsync());
        }


        #endregion
        #region --------------------------------------------------- INIT


        protected abstract void Init();
        
        private IEnumerator InitAsync() 
        {
            yield return new WaitUntil(() => Global.IsInitialized);

            Init();
        }


        #endregion
    }
}