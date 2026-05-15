using UnityEngine;

namespace LLib
{
    public abstract class UIPopup : UIScene
    {
        [SerializeField] private bool _isGlobal = false;
        [SerializeField] private bool _isModal = true;
        
        private PopupManager _popupManager; 
        
        public bool IsGlobal => _isGlobal;
        public bool IsModal => _isModal;

        
        protected virtual void OnDestroy()
        {
            Services.Get<PopupManager>()?.Remove(this);
        }
        
        
        public override void Init()
        {
            base.Init();
            
            gameObject.SetActive(false);
            
            Services.Get<PopupManager>()?.Add(this);
        }
    }
}

