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

        
        
        
        public virtual void Init()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
        }
        
        
        public abstract void Refresh();

        
        public void Open()
        {
            if (IsOpened) 
                return;
            
            IsOpened = true;
            
            Show();
            Refresh();
        }


        public void Close()
        {
            if (!IsOpened) 
                return;
            
            IsOpened = false;
            
            Hide();
        }


        protected virtual void Show()
        {
            gameObject.SetActive(true);
        }

        
        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
    }
}
