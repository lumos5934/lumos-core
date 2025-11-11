using UnityEngine.Audio;

namespace LumosLib
{
    public interface IAudioManager
    {
        public void SetVolume(string groupName, float volume);
        public void PlayBGM(int bgmType, string assetName);
        public void PlaySFX(string assetName);
        public void StopBGM(int bgmType);
        public void StopSFXAll();
        public void StopAll();
        public void PauseBGM(int bgmType, bool enable);
        public void PauseSFXAll(bool enable);
        public void PauseAll(bool enable);
    }
}