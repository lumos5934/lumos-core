using System.Collections.Generic;
using UnityEngine;

namespace LumosLib
{
    public class UIManager : MonoBehaviour, IUIManager, IPreInitialize
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public int PreInitOrder => (int)PreInitializeOrder.UI;
        public bool PreInitialized => true;
        
        
        #endregion
        #region >--------------------------------------------------- FIELDS


        protected Dictionary<int, UIBase> _ui = new();
        protected Dictionary<int, UIBase> _globalUIPrefabs = new();
        


        #endregion
        #region >--------------------------------------------------- UNITY

        protected virtual void Awake()
        {
            
            var uiGlobalPrefabs =  Global.GetInternal<IResourceManager>().LoadAll<UIBase>("");

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var key = uiGlobalPrefabs[i].ID;
                var value = uiGlobalPrefabs[i];

                _globalUIPrefabs[key] = value;
            }
            
            Global.Register<IUIManager>(this);
            
            DontDestroyOnLoad(gameObject);
        }

        #endregion
        #region >--------------------------------------------------- GET & SET


        public void SetEnable<T>(int id, bool enable) where T : UIBase
        {
            var ui = Get<T>(id);

            if (ui == null) return;

            ui.SetEnable(enable);
        }

        public virtual T Get<T>(int id) where T : UIBase
        {
            if (_ui.TryGetValue(id, out var ui))
            {
                return ui as T;
            }
            else
            {
                return CreateUI<T>(id);
            }
        }
        
        private T CreateUI<T>(int id) where T : UIBase
        {
            if (!_globalUIPrefabs.TryGetValue(id, out var prefab)) return null;

            var createdUI = Instantiate(prefab, transform);
            
            Register(createdUI);
            
            createdUI.gameObject.SetActive(false);
            
            return createdUI as T;
        }


        #endregion
        #region >--------------------------------------------------- REGISTER


        protected void Register<T>(T ui)  where T : UIBase
        {
            _ui[ui.ID] = ui;
        }

        
        #endregion
    }
}