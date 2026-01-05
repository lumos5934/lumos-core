using System.Collections.Generic;
using UnityEngine;
using TriInspector;

namespace LumosLib
{
    public class LumosLibSettings : ScriptableObject
    {
        [field: Title("Preload")]
        [field: SerializeField, LabelText("Use")] public bool UsePreload { get; private set; }
        [field: SerializeField, ShowIf("UsePreload")] public List<GameObject> PreloadObjects { get; private set; } = new();
    }
}
