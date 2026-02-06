using UnityEngine;

namespace LumosLib
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIPanel : UIBase
    {
        public bool IsOpened { get; protected set; }
        public Canvas Canvas { get; private set; }
        
        
        public virtual void Open() => IsOpened = true;
        public virtual void Close() => IsOpened = false;
        public  override void Init()
        {
            base.Init();
            
            Canvas =  GetComponent<Canvas>();

            var childUIs = GetComponentsInChildren<UIBase>();
            foreach (var ui in childUIs)
            {
                if (ui.gameObject == gameObject) 
                    continue;
                
                ui.Init();
            }
        }
    }
}