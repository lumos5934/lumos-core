using UnityEngine;
using UnityEngine.UI;

namespace LumosLib
{
    [RequireComponent(typeof(Canvas)),
     RequireComponent(typeof(CanvasScaler)),
     RequireComponent(typeof(GraphicRaycaster))]
    public abstract class UIPanel : UIBase
    {
        protected Canvas _canvas;
        
        public override void Init()
        {
            base.Init();
            
            _canvas =  GetComponent<Canvas>();
            
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
            _canvas.worldCamera = cam;
        }

        public void SetOrder(int order)
        {
            _canvas.sortingOrder = order;
        }
    }
}