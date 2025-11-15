using DG.Tweening;
using TriInspector;

namespace LumosLib
{
    public class ScaleTweenPresetAnimator : BaseTweenPresetAnimator<ScaleTweenPreset>
    {
        [PropertySpace(10f)]
        public override Tweener GetTweener(string key)
        {
            var preset = GetPreset(key);
            
            if (preset.GetUseInitScale())
            {
                transform.localScale = preset.GetInitScale();
            }
            
            return OnGetTweener(preset, transform.DOScale(preset.GetScale(), preset.GetDuration()));
        }
    }
}