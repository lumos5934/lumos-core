using System.Collections.Generic;
using UnityEngine;
using TriInspector;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "LumosLibSettings", menuName = "[ LumosLib ]/Scriptable Objects/Settings", order = int.MinValue)]
    public class LumosLibSettings : ScriptableObject
    {
        [SerializeField] private bool _usePreInitialize;

        [PropertySpace(10f)] 
        [SerializeField] private List<GameObject> _preloadObjects;

        public bool UsePreInitialize => _usePreInitialize;
        public List<GameObject> PreloadObjects => _preloadObjects;
    }
}
