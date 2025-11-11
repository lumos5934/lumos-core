using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LumosLib
{
    public class UITweenAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region >--------------------------------------------------- FIELD

        
        [Title("Event")]
        [SerializeField] private bool _usePointerEnter;
        [SerializeField, ShowIf("_usePointerEnter"), LabelText("Presets")] private BaseTweenPreset[] _pointerEnterTweenPresets;
        
        [SerializeField] private bool _usePointerExit;
        [SerializeField, ShowIf("_usePointerExit"), LabelText("Presets")] private BaseTweenPreset[] _pointerExitTweenPresets;
    
        protected Sequence _sequence;
        
        #endregion
        #region >--------------------------------------------------- UNITY

        protected virtual void Awake()
        {
            InitPreset(_pointerEnterTweenPresets);
            InitPreset(_pointerExitTweenPresets);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_usePointerEnter)
            {
                PlaySequence(_pointerEnterTweenPresets);
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_usePointerExit)
            {
                PlaySequence(_pointerExitTweenPresets);
            }
        }
        

        #endregion
        #region >--------------------------------------------------- UNITY


        protected void InitPreset(BaseTweenPreset[] presets)
        {
            foreach (var preset in presets)
            {
                preset.InitComponent(gameObject);
            }
        }
        
        
        #endregion
        #region >--------------------------------------------------- SEQUENCE


        protected void PlaySequence(BaseTweenPreset[] presets)
        {
            if (presets.Length == 0) return;
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
                
            foreach (var preset in presets)
            {
                var tween = preset.GetTween();
                if (tween != null)
                {
                    _sequence.Join(tween);
                }
            }

            _sequence.Play();
        }
        
        
        #endregion
    }
}   