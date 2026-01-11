using System.Collections.Generic;
using UnityEngine;
using TriInspector;

namespace Lumos
{
    [CreateAssetMenu(fileName = "LumosSettings", menuName = "SO/Settings", order = int.MinValue)]
    public class LumosSettings : ScriptableObject
    {
        [field: Title("Pre Initialize Settings")]
        [field: SerializeField, LabelText("Use")] public bool UsePreInit { get; private set; }
        [field: SerializeField, ShowIf("UsePreInit")] public List<GameObject> PreloadObjects { get; private set; } = new();
    }
}
