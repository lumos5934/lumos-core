using UnityEngine;

namespace LumosLib
{
    public abstract class UIPopup : UIPanel
    {
        [SerializeField] private bool _isGlobal;
        private BasePopupManager _baseManager; 
        
        public bool IsGlobal => _isGlobal;
        public bool IsOpened { get; protected set; }

        public override void Init()
        {
            base.Init();
            _baseManager = GlobalService.Get<IPopupManager>() as  BasePopupManager;
            _baseManager?.Register(this);
        }

        private void OnDestroy()
        {
            _baseManager?.Unregister(this);
        }
    
        internal void Open()
        {
            IsOpened = true;
            OnOpen();
        }

        internal void Close()
        {
            IsOpened = false;
            OnClose();
        }
        
        protected virtual void OnOpen() => gameObject.SetActive(true);
        protected virtual void OnClose() => gameObject.SetActive(false);
    }
}