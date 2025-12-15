using LumosLib;
using UnityEngine;

namespace LumosLib
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIBase : MonoBehaviour
    {
        #region >--------------------------------------------------- PROPERTIES

        public bool IsEnabled { get; protected set; }
        protected Canvas Canvas => _canvas;
        protected CanvasGroup CanvasGroup => _canvasGroup;


        #endregion
        #region >--------------------------------------------------- FIELDS


        private Canvas _canvas;
        private CanvasGroup _canvasGroup;


        #endregion
        #region >--------------------------------------------------- UNITY


        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        
        #endregion
        #region >--------------------------------------------------- Set


        public virtual void SetEnable(bool enable)
        {
            IsEnabled = enable;
        }


        #endregion
    }
}
