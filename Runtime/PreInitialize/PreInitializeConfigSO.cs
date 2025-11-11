using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace LumosLib
{
    public class PreInitializeConfigSO : ScriptableObject
    {
        public enum TableType
        {
            None,
            GoogleSheet,
            Local
        }
        
        [field: Header("Data")]
        [field: SerializeField] public TableType SelectedTableType { get; private set; }
        [field: SerializeField] public string TablePath { get; private set; }
        
        
        [field: Header("Audio")]
        [field: SerializeField] public AudioMixer Mixer { get; private set; }
        [field: SerializeField] public AudioPlayer AudioPlayerPrefab { get; private set; }


        [field: Header("PreInitialize")]
        [field: SerializeField] public List<MonoBehaviour> PreInitializes { get; private set; } = new();


        private void Awake()
        {
#if UNITY_EDITOR
            SelectedTableType = TableType.None;
            
            if (PreInitializes.Count == 0)
            {
                PreInitializes.Add(Resources.Load<DataManager>(nameof(DataManager)));
                PreInitializes.Add(Resources.Load<PoolManager>(nameof(PoolManager)));
                PreInitializes.Add(Resources.Load<AudioManager>(nameof(AudioManager)));
                PreInitializes.Add(Resources.Load<UIManager>(nameof(UIManager)));
                PreInitializes.Add(Resources.Load<ResourceManager>(nameof(ResourceManager)));
            }
            
            if (AudioPlayerPrefab == null)
            {
                AudioPlayerPrefab = Resources.Load<AudioPlayer>(nameof(AudioPlayer));
            }
        }
#endif
    }
}
