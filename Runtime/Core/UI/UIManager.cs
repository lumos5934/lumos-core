using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TriInspector;

namespace LumosLib
{
    public class UIManager : MonoBehaviour, IUIManager, IPreInitializable
    {
        #region >--------------------------------------------------- FIELDS

        
        private Dictionary<Type, UIBase> _createdUIs = new();
        private Dictionary<Type, UIBase> _prefabUIs = new();
        
        [Title("REQUIREMENT")]
        [ShowInInspector, HideReferencePicker, ReadOnly, LabelText("IResourceManager")] private IResourceManager _resourceManager;


        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public IEnumerator InitAsync(Action<bool> onComplete)
        {
            _resourceManager = GlobalService.Get<IResourceManager>();
            if (_resourceManager == null)
            {
                onComplete?.Invoke(false);
                yield break;
            }
            
            var uiGlobalPrefabs =  _resourceManager.LoadAll<UIBase>("");

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var value = uiGlobalPrefabs[i];

                _prefabUIs[uiGlobalPrefabs[i].GetType()] = value;
            }
            
            GlobalService.Register<IUIManager>(this);
            DontDestroyOnLoad(gameObject);
            
            onComplete?.Invoke(true);
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