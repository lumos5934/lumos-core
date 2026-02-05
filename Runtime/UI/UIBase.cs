using LumosLib;
using UnityEngine;

namespace LumosLib
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIBase : MonoBehaviour
    {
        #region >--------------------------------------------------- PROPERTIES

        
        public bool IsEnabled { get; protected set; }
        protected CanvasGroup CanvasGroup { get; private set; }


        #endregion
        #region >--------------------------------------------------- UNITY


        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
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
