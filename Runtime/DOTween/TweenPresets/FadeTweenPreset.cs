using TriInspector;
using UnityEngine;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "FadeTweenPreset", menuName = "[ ✨Lumos Lib ]/Scriptable Object/Tween Preset/Fade", order = int.MinValue)]
    public class FadeTweenPreset : BaseTweenPreset
    {
  
        [Title("Fade")] 
        [SerializeField] private float _fade;
        
        [SerializeField] private bool _useInitFade;
        [SerializeField, ShowIf("_useInitFade")] private float _initFade;
        

        #region >--------------------------------------------------- GET


        public float GetFade()
        {
            return _fade;
        }

        public bool GetUseInitFade()
        {
            return _useInitFade;
        }

        public float GetInitFade()
        {
            return _initFade;
        }

        #endregion
    }
}