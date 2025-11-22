using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LumosLib
{
    public abstract class BaseUIManager : MonoBehaviour, IUIManager, IPreInitializer
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public int PreInitOrder => (int)PreInitializeOrder.UI;
        
        
        #endregion
        #region >--------------------------------------------------- FIELDS


        protected Dictionary<Type, UIBase> _createdUIs = new();
        protected Dictionary<Type, UIBase> _prefabUIs = new();


        #endregion
        #region >--------------------------------------------------- UNITY

        
        protected virtual void Awake()
        {
            GlobalService.Register<IUIManager>(this);
            
            DontDestroyOnLoad(gameObject);
        }
        

        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public virtual IEnumerator InitAsync()
        {
            var uiGlobalPrefabs =  GlobalService.GetInternal<IResourceManager>().LoadAll<UIBase>("");

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var value = uiGlobalPrefabs[i];

                _prefabUIs[uiGlobalPrefabs[i].GetType()] = value;
            }
            
            yield break;
        }
        
        
        #endregion
        #region >--------------------------------------------------- GET & SET

        
        public virtual void SetEnable<T>(bool enable) where T : UIBase
        {
            var ui = Get<T>();

            if (ui == null) return;

            ui.SetEnable(enable);
        }
        
        public virtual void SetToggle<T>() where T : UIBase
        {
            var ui = Get<T>();

            if (ui == null) return;

            ui.SetEnable(!ui.IsEnabled);
        }

        public virtual T Get<T>() where T : UIBase
        {
            if (_createdUIs.TryGetValue(typeof(T), out var ui))
            {
                return ui as T;
            }
            
            return CreateUI<T>();
        }
        
        
        #endregion
        #region >--------------------------------------------------- CREATE

        
        protected virtual T CreateUI<T>() where T : UIBase
        {
            var type = typeof(T);
            
            if (!_prefabUIs.TryGetValue(type, out var prefab)) return null;

            var createdUI = Instantiate(prefab, transform);
            
            _createdUIs[type] = createdUI;
            
            createdUI.gameObject.SetActive(false);
            
            return createdUI as T;
        }
        

        #endregion
    }
}