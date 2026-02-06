using UnityEngine;

namespace LumosLib
{
    [RequireComponent(typeof(CanvasGroup),
        typeof(RectTransform))]
    public abstract class UIBase : MonoBehaviour
    {
        protected CanvasGroup CanvasGroup { get; private set; }
        public RectTransform RectTransform { get; private set; }

        public virtual void Init()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
        }
    }
}
