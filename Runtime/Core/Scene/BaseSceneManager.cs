using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LumosLib
{
    public abstract class BaseSceneManager<T> : MonoBehaviour where T : BaseSceneManager<T>
    {
        #region --------------------------------------------------- UNITY


        protected virtual void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        

        protected virtual void OnDestroy()
        {
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