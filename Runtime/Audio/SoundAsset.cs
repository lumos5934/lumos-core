using UnityEngine;
using UnityEngine.Audio;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "SoundAsset", menuName = "SO/Sound Asset")]
    public class SoundAsset : ScriptableObject
    {
        [field: SerializeField] public AudioMixerGroup MixerGroup { get; set; }
        [field: SerializeField] public AudioClip Clip { get; set; }

        [field: Range(0, 1)]
        [field: SerializeField] public float VolumeFactor { get; set; } = 1;
        [field: SerializeField] public bool IsLoop { get; set; }
    }
}