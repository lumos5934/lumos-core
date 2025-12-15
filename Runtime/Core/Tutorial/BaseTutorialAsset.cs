using UnityEngine;

namespace LumosLib
{
    public abstract class BaseTutorialAsset : ScriptableObject
    {
        [field: SerializeField] public string Description { get; private set; } = "";

        public abstract BaseTutorial Create();
    }
}

