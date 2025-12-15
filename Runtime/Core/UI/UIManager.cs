using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LumosLib
{
    public class UIManager : MonoBehaviour, IUIManager, IPreInitializer
    {
        #region >--------------------------------------------------- FIELDS


        private Dictionary<Type, UIBase> _createdUIs = new();
        private Dictionary<Type, UIBase> _prefabUIs = new();


        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public IEnumerator InitAsync()
        {
            var uiGlobalPrefabs =  GlobalService.GetInternal<IResourceManager>().LoadAll<UIBase>("");

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var value = uiGlobalPrefabs[i];

                _prefabUIs[uiGlobalPrefabs[i].GetType()] = value;
            }
            
            GlobalService.Register<IUIManager>(this);
            DontDestroyOnLoad(gameObject);
            
            yield break;
        }
        
        
        #endregion
        #region >--------------------------------------------------- GET & SET

        
        public virtual T Get<T>() where T : UIBase
        {
            if (_createdUIs.TryGetValue(typeof(T), out var ui))
            {
                return ui as T;
            }
            
            return CreateUI<T>();
        }
        
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