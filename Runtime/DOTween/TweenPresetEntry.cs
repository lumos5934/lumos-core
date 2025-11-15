using System;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    [Serializable]
    public class TweenPresetEntry<T> where T : BaseTweenPreset
    {
        [Group("Key"), HideLabel]
        [SerializeField] private string _key;
        
        [Group("Preset"), HideLabel]
        [SerializeField] private T _preset;

        public T GetPreset()
        {
            return _preset;
        }

        public string GetKey()
        {
            return _key;
        }
    }
}