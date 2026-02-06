using UnityEngine;

namespace LumosLib
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIPanel : UIBase
    {
        public bool IsOpened { get; protected set; }
        protected Canvas Canvas { get; private set; }
        
        [SerializeField] protected Camera _camera;
        
        
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

        public void SetCamera(Camera cam)
        {
            _camera = cam;
            Canvas.worldCamera = cam;
        }

        public void SetOrder(int order)
        {
            Canvas.sortingOrder = order;
        }
    }
}