using UnityEngine;

namespace Lumos.DevPack
{
    public abstract class SingletonGlobal<T> : SingletonScene<T> where T : SingletonGlobal<T>
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
 
  
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}
