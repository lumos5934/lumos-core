using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using TriInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[assembly: InternalsVisibleTo("LumosLib.Editor")]
namespace LumosLib
{
    [CreateAssetMenu(fileName = "LumosLibSettings", menuName = "[ LumosLib ]/Scriptable Objects/Settings", order = int.MinValue)]
    public class LumosLibSettings : ScriptableObject
    {
        private static LumosLibSettings _instance;
        public static LumosLibSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEditor.PlayerSettings.GetPreloadedAssets()
                        .OfType<LumosLibSettings>().FirstOrDefault();
                }
                return _instance;
            }
        }
        
        [Title("Preload")]
        [SerializeField] private bool _usePreInitialize;

        [PropertySpace(10f)] 
        [SerializeField] private List<GameObject> _preloadObjects;

#if UNITY_EDITOR
        [Button]
        public void RegisterToPlayerSettings()
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (!preloadedAssets.Contains(this))
            {
                preloadedAssets.Add(this);
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                DebugUtil.Log("Success registered","LumosLibSettings");
            }
            else
            {
                DebugUtil.Log("Already registered","LumosLibSettings");
            }
        }
        
        [Button]
        public void UnregisterToPlayerSettings()
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Contains(this))
            {
                preloadedAssets.Remove(this);
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                DebugUtil.Log("Success unregistered","LumosLibSettings");
            }
            else
            {
                DebugUtil.Log("Not registered","LumosLibSettings");
            }
        }
#endif
        public bool UsePreInitialize => _usePreInitialize;
        public List<GameObject> PreloadObjects => _preloadObjects;


        /// <summary>
        /// TetTool Settings
        /// </summary>
        internal int TitleFontSize = 25;
        internal Color TitleFontColor = new (1f, 0.9f, 0.9f);
        internal Color TitleFontShadowColor = new (0.15f, 0.1f, 0f, 0.8f);
        internal Color TitleUnderLineColor =  new (1, 1, 1, 0.15f);
        internal Color TitleUnderLineHighlightColor = new (1f, 0.85f, 0.4f, 0.6f);
        
        internal int ButtonFontSize = 12;
        internal float ButtonHeight = 25;
        internal float ButtonWidth = 90;
        
        internal Color ButtonNormalColor = new (1f, 0.80f, 0.6f, 0.6f);
        internal Color ButtonSelectedColor = new (0.40f, 0.40f, 0.40f, 1f);
        internal Color ButtonHighlightColor = new (1f, 0.85f, 0.4f, 0.9f);
        internal Color ButtonFontNormalColor = Color.white; 
        internal Color ButtonFontHoverColor = new (0.5f, 0.8f, 0.7f, 1);
        internal Color ContentsBackgroundColor = new (0f, 0f, 0f, 0.7f);
    }
}
