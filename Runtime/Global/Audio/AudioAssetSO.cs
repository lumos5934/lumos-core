using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Lumos.DevPack
{
    [CreateAssetMenu(fileName = "SoundAsset", menuName = "Scriptable objects/Sound Asset")]
    public class AudioAssetSO : ScriptableObject
    {
        [field: SerializeField] public AudioMixerGroup MixerGroup { get; set; }
        [field: SerializeField] public AudioClip Clip { get; set; }
        [field: SerializeField] public float VolumeFactor { get; set; }

        public virtual int GetID()
        {
            return name.GetHashCode();
        }
    }
}