using UnityEngine;

namespace Lumos.DevPack
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIBase : MonoBehaviour
    {
        #region >--------------------------------------------------- PROPERTIES

        
        public abstract int ID { get; }
        protected UIManager UIManager => _uiManager;
        protected Canvas Canvas => _canvas;
        protected CanvasGroup CanvasGroup => _canvasGroup;


        #endregion

        #region >--------------------------------------------------- FIELDS


        private UIManager _uiManager;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;


        #endregion

        #region >--------------------------------------------------- UNITY


        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected void OnDestroy()
        {
            _uiManager?.Unregister(ID);
        }

        
        #endregion

        #region >--------------------------------------------------- INIT


        public virtual void Init()
        {
            _uiManager = Global.Get<UIManager>();
            _uiManager.Register(ID, this);
        }


        #endregion

        #region >--------------------------------------------------- Set


        public abstract void SetEnable(bool enable);


        #endregion
    }
}