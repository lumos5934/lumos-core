using UnityEngine;
using DG.Tweening;
using TriInspector;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "ScaleTweenPreset", menuName = "[ ✨Lumos Lib ]/Scriptable Object/Tween Preset/Scale", order = int.MinValue)]
    public class ScaleTweenPreset : BaseTweenPreset
    {
        [PropertySpace(20f)]
        [Title("Scale")]
        [SerializeField] private Vector2 _targetScale;
        [SerializeField] private bool _useInitialScale;
        [SerializeField, ShowIf("_useInitialScale")] private Vector2 _initialScale;

        private Transform _targetTransform;
        
        public override void InitComponent(GameObject targetObject)
        {
            _targetTransform = targetObject.transform;
        }

        protected override Tween SetTween()
        {
            if (_targetTransform != null)
            {
                if (_useInitialScale)
                {
                    _targetTransform.localScale = _initialScale;
                }
                
                return _targetTransform.DOScale(_targetScale, Duration);
            }
            
            return null;
        }
    }
}