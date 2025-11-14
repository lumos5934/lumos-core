using System.Collections;
using UnityEngine.SceneManagement;

namespace LumosLib
{
    public abstract class BaseSceneManager<T> : SingletonScene<BaseSceneManager<T>> where T : BaseSceneManager<T>
    {
        #region --------------------------------------------------- UNITY


        protected override void Awake()
        {
            base.Awake();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            GlobalService.Unregister<T>();
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        #endregion
        #region --------------------------------------------------- INIT


        protected virtual void OnInitAsync()
        {
            GlobalService.Register((T)this);
        }
        
        private IEnumerator InitAsync() 
        {
            if (!Project.Initialized)
            { 
                yield return Project.InitAsync();
            }

            OnInitAsync();
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(InitAsync());
        }


        #endregion
    }
}