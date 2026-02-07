using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace LumosLib
{
    public abstract class BasePopupManager : MonoBehaviour, IPopupManager, IPreInitializable
    {
        #region >--------------------------------------------------- FIELD

        
        protected Dictionary<Type, UIPopup> _popupCache = new();
        protected List<UIPopup> _openedPopups = new();
        protected Camera _camera;
       
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public async UniTask<bool> InitAsync()
        {
            return await OnInitAsync();
        }

        protected abstract UniTask<bool> OnInitAsync();
       
        
        
        #endregion
        #region >--------------------------------------------------- CORE
        
        
        internal void Register(UIPopup popup)
        {
            var type = popup.GetType();
            _popupCache.TryAdd(type, popup);
        }

        internal void Unregister(UIPopup popup)
        {
            var type = popup.GetType();
            _popupCache.Remove(type);
        }

        
        public T Get<T>() where T : UIPopup
        {
            var type = typeof(T);
            
            foreach (var popup in _openedPopups)
            {
                if (popup.GetType() == type)
                {
                    return popup as T;
                }
            }
            
            return null;
        }
        
        
        public T Open<T>()  where T : UIPopup => OnOpen<T>();
        public void Close() =>  OnClose();
        public void Close<T>() where T : UIPopup => OnClose<T>();

        protected abstract T OnOpen<T>() where T : UIPopup;
        protected abstract void OnClose();
        protected abstract void OnClose<T>() where T : UIPopup;

        public void CloseAll()
        {
            foreach (var popup in _openedPopups)
            {
                popup.Close();
            }
            
            _openedPopups.Clear();
        }

        
        #endregion
    }
}