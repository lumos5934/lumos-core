using System;
using UnityEngine;

namespace LLib
{
    [RequireComponent(typeof(CanvasGroup),
        typeof(RectTransform))]
    public abstract class UIBase : MonoBehaviour
    {
        public bool IsOpened { get; protected set; }
        public CanvasGroup CanvasGroup { get; private set; }
        public RectTransform RectTransform { get; private set; }

        
        protected abstract void Refresh();
        
        
        public virtual void Init()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
        }

        
        public virtual void Open<T>(Action<T> onBeforeOpen) where T : UIBase
        {
            if (IsOpened) 
                return;
            
            onBeforeOpen?.Invoke(this as T);
            
            IsOpened = true;
            
            OnShow();
            
            Refresh();
        }


        public virtual void Close()
        {
            if (!IsOpened) 
                return;
            
            IsOpened = false;
            
            OnHide();
        }


        protected virtual void OnShow()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnHide()
        {
            gameObject.SetActive(false);
        }
        
    }
}
