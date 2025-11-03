using UnityEngine;

namespace Lumos.DevKit
{
    public abstract class SingletonGlobal<T> : MonoBehaviour where T : SingletonGlobal<T>
    {
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting) return null;

                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }
            
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
        private static T _instance;
    
        private static bool applicationIsQuitting = false;
 
  
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
        }
        
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                _instance = null;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}
