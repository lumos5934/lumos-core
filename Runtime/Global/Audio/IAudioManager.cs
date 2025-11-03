using UnityEngine.Audio;

namespace LumosLib.Core
{
    public interface IAudioManager : IGlobal
    {
        public void SetMixer(AudioMixer mixer);
        public void SetVolume(string groupName, float volume);

        public void PlayBGM(int bgmType, int assetId);
        public void PlaySFX(int assetId);
        public void StopBGM(int bgmType);
        public void StopAll();
        public void PauseBGM(int bgmType, bool enable);
        public void PauseAll(bool enable);
        public void ReleaseAudioPlayer(AudioPlayer player);
    }
}