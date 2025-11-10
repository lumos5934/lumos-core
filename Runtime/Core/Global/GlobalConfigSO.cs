using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "Global Config", menuName = "[ ✨Lumos Lib Asset ]/Global Config", order = 0) ]
    public class GlobalConfigSO : ScriptableObject
    {
        [field: Header("Audio")]

        [field: SerializeField] public AudioMixerGroup Mixer { get; private set; }


        [field: Header("PreInitialize")]
        [field: SerializeField] public List<MonoBehaviour> PreInitializes { get; private set; } = new();

        private void Awake()
        {
#if UNITY_EDITOR
            if (PreInitializes.Count == 0)
            {
                PreInitializes.Add(Resources.Load<DataManager>(nameof(DataManager)));
                PreInitializes.Add(Resources.Load<PoolManager>(nameof(PoolManager)));
                PreInitializes.Add(Resources.Load<ResourceManager>(nameof(ResourceManager)));
                PreInitializes.Add(Resources.Load<UIManager>(nameof(UIManager)));
                PreInitializes.Add(Resources.Load<AudioManager>(nameof(AudioManager)));
            }
#endif
        }
    }
}
