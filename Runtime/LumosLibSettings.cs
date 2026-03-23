using System.Collections.Generic;
using UnityEngine;
using TriInspector;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "LumosLibSettings", menuName = "[ LumosLib ]/Scriptable Objects/Settings", order = int.MinValue)]
    public class LumosLibSettings : ScriptableObject
    {
        [Title("Test Tool")]
        [Header("Title")]
        [SerializeField] private Color _nameColor = new (1f, 0.9f, 0.9f);
        [SerializeField] private Color _nameShadowColor = new (0.15f, 0.1f, 0f, 0.8f);
        [SerializeField] private Color _underLineHighlightColor = new (1f, 0.85f, 0.4f, 0.6f);
        [SerializeField] private Color _underLineHighlightShadowColor = new (0.6f, 0.45f, 0f, 0.6f);
        public Color NameColor => _nameColor;
        public Color NameShadowColor => _nameShadowColor;
        public Color UnderLineHighlightColor => _underLineHighlightColor;
        public Color UnderLineHighlightShadowColor => _underLineHighlightShadowColor;
        
        
        [Header("Element Button")]
        [SerializeField, LabelText("NameNormalColor")] private Color _buttonNameNormalColor = Color.white; 
        [SerializeField, LabelText("NameHoverColor")] private Color _buttonNameHoverColor = new (0.5f, 0.8f, 0.7f, 1);
        [SerializeField, LabelText("NormalColor")] private Color _buttonNormalColor = new (1f, 0.80f, 0.6f, 0.6f);
        [SerializeField, LabelText("SelectedColor")] private Color _buttonSelectedColor = new (0.40f, 0.40f, 0.40f, 1f);
        [SerializeField, LabelText("HighlightColor")] private Color _buttonHighlightColor = new (1f, 0.85f, 0.4f, 0.9f);
       
        public Color ButtonNameNormalColor => _buttonNameNormalColor;
        public Color ButtonNameHoverColor => _buttonNameHoverColor;
        public Color ButtonNormalColor => _buttonNormalColor;
        public Color ButtonSelectedColor => _buttonSelectedColor;
        public Color ButtonHighlightColor => _buttonHighlightColor;
        
        
        [PropertySpace(20f)]
        [Title("Preload")]
        [SerializeField] private bool _usePreInitialize;

        [PropertySpace(10f)] 
        [SerializeField] private List<GameObject> _preloadObjects;

        public bool UsePreInitialize => _usePreInitialize;
        public List<GameObject> PreloadObjects => _preloadObjects;

    }
}
