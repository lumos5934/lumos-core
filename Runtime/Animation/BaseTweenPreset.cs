using UnityEngine;
using DG.Tweening;
using TriInspector;

namespace LumosLib
{
    //[CreateAssetMenu(fileName = "#Name#TweenPreset", menuName = "[ ✨Lumos Lib ]/Scriptable Object/Tween Preset/#Name#")]
    public abstract class BaseTweenPreset : ScriptableObject
    {
        protected float Duration
        {
            get
            {
                float duration = _useRandomDuration 
                    ? _duration * Random.Range(1 - _durationRandomFactor, 1 + _durationRandomFactor) 
                    : _duration;

                return duration;
            }
        }

        [Title("Base")]
        [SerializeField] private Ease _ease;
        [SerializeField] private float _duration;
        [SerializeField] private bool _useRandomDuration;
        [SerializeField, ShowIf("_useRandomDuration"), Range(0, 1)] private float _durationRandomFactor;

        [PropertySpace(20f)] 
        [Title("Loop")] 
        [SerializeField] private bool _isLoop;
        [SerializeField, ShowIf("_isLoop")] private LoopType _loopType;
        [SerializeField, ShowIf("_isLoop")] private int _loopCount;


        public abstract void InitComponent(GameObject targetObject);

        public Tween GetTween()
        {
            var tween = SetTween()?.SetEase(_ease);
            
            return _isLoop ? tween.SetLoops(_loopCount, _loopType) : tween;
        }
        
        protected abstract Tween SetTween();
    }
}