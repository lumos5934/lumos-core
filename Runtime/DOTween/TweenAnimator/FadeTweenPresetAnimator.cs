using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LumosLib
{
    public class FadeTweenPresetAnimator : BaseTweenPresetAnimator<FadeTweenPreset>
    {
        private enum FadeType
        {
            SpriteRenderer,
            Image,
            CanvasGroup
        }
        
        [PropertySpace(10f)]
        [Title("Fade")]
        [SerializeField] private FadeType _fadeType;
     
        [ShowIf("_fadeType",  FadeType.SpriteRenderer)]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [ShowIf("_fadeType",  FadeType.Image)]
        [SerializeField] private Image _image;
        
        [ShowIf("_fadeType",  FadeType.CanvasGroup)]
        [SerializeField] private CanvasGroup _canvasGroup;


        public override Tweener GetTweener(string key)
        {
            var preset = GetPreset(key);


            switch (_fadeType)
            {
                case FadeType.SpriteRenderer :
                    if (_spriteRenderer == null) return null;
                    
                    if (preset.GetUseInitFade())
                    {
                        var color = _spriteRenderer.color;
                        color.a = preset.GetInitFade();
                        _spriteRenderer.color = color;
                    }
                    
                    return OnGetTweener(preset, _spriteRenderer.DOFade(preset.GetFade(), preset.GetDuration()));
                
                case FadeType.Image :
                    if (_image == null) return null;
                    
                    if (preset.GetUseInitFade())
                    {
                        var color = _image.color;
                        color.a = preset.GetInitFade();
                        _image.color = color;
                    }
                    return OnGetTweener(preset, _image.DOFade(preset.GetFade(), preset.GetDuration()));
                
                case FadeType.CanvasGroup :
                    if (_canvasGroup == null) return null;
                    
                    if (preset.GetUseInitFade())
                    {
                        _canvasGroup.alpha = preset.GetInitFade();
                    }
                    return OnGetTweener(preset, _canvasGroup.DOFade(preset.GetFade(), preset.GetDuration()));
            }

            return null;
        }
    }
}