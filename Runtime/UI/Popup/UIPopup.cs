using UnityEngine;

namespace LumosLib
{
    public abstract class UIPopup : UIPanel
    {
        public bool IsGlobal => _isGlobal;
        
        [SerializeField] private bool _isGlobal;
        public override void Init()
        {
            base.Init();
            
            
            GlobalService.Get<IPopupManager>()?.Register(this);
        }

        void OnDestroy()
        {
            GlobalService.Get<IPopupManager>()?.Unregister(this);
        }
    }
}