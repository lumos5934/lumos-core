using System.Collections.Generic;
using UnityEngine;
using TriInspector;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "LumosLibSettings", menuName = "[ LumosLib ]/SO/Settings", order = int.MinValue)]
    public class LumosLibSettings : ScriptableObject
    {
        [field: Title("Pre Initialize")]
        [field: SerializeField, LabelText("Use")] public bool UsePreInit { get; private set; }
        [field: SerializeField, ShowIf("UsePreInit")] public List<GameObject> PreloadObjects { get; private set; } = new();
    }
}
