using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public class UIManager : MonoBehaviour, IBootable
    {
        #region >--------------------------------------------------- PROPERTIES

        
        public int Order => (int)BootsOrder.UI;
        public bool IsInitialized { get; protected set; }


        #endregion
        #region >--------------------------------------------------- PROPERTIES


        private Dictionary<int, UIBase> _uiBases = new();
        private Dictionary<Type, UIGlobal> _uiGlobalPrefabs = new();


        #endregion
        #region >--------------------------------------------------- INIT


        public Task InitAsync()
        {
            var uiGlobalPrefabs = Resources.LoadAll<UIGlobal>("");

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var key = uiGlobalPrefabs[i].GetType();
                var value = uiGlobalPrefabs[i];

                _uiGlobalPrefabs[key] = value;
            }
            
            SetEnable<TestUI>(0, true);
            
            
            IsInitialized = true;
            
            return Task.CompletedTask;
        }
  

        #endregion
        #region >--------------------------------------------------- GET & SET


        public void SetEnable<T>(int key, bool enable) where T : UIBase
        {
            var ui = Get<T>(key);

            if (ui == null) return;

            ui.SetEnable(enable);
        }

        public T Get<T>(int key) where T : UIBase
        {
            if (_uiBases.TryGetValue(key, out var ui))
            {
                return ui as T;
            }

            return TryCreateGlobalUI<T>();
        }


        #endregion
        #region >--------------------------------------------------- REGISTER


        public void Register<T>(int key, T ui) where T : UIBase
        {
            _uiBases[key] = ui;
        }

        public void Unregister(int key)
        {
            _uiBases.Remove(key);
        }


        #endregion
        #region >--------------------------------------------------- GLOBAL


        private T TryCreateGlobalUI<T>() where T : UIBase
        {
            var type = typeof(T);

            if (!_uiGlobalPrefabs.TryGetValue(type, out var prefab)) return null;

            var ui = Instantiate(prefab, transform);

            Register(ui.ID, ui);

            return ui as T;
        }


        #endregion
    }
}