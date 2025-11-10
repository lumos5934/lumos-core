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

        
        public void Init()
        {
            SelectedTableType = TableType.None;
            
            
            if (PreInitializes.Count == 0)
            {
                PreInitializes.Add(
                    AssetDatabase.LoadAssetAtPath<DataManager>(Constant.PathRuntimeSamples + "/DataManager.prefab"));
                PreInitializes.Add(
                    AssetDatabase.LoadAssetAtPath<PoolManager>(Constant.PathRuntimeSamples + "/PoolManager.prefab"));
                PreInitializes.Add(
                    AssetDatabase.LoadAssetAtPath<AudioManager>(Constant.PathRuntimeSamples + "/AudioManager.prefab"));
                PreInitializes.Add(
                    AssetDatabase.LoadAssetAtPath<UIManager>(Constant.PathRuntimeSamples + "/UIManager.prefab"));
                PreInitializes.Add(
                    AssetDatabase.LoadAssetAtPath<ResourceManager>(Constant.PathRuntimeSamples + "/ResourceManager.prefab"));
            }
            
            if (AudioPlayerPrefab == null)
            {
                AudioPlayerPrefab =
                    AssetDatabase.LoadAssetAtPath<AudioPlayer>(Constant.PathRuntimeSamples + "/AudioPlayer.prefab");
            }
        }
    }
}
