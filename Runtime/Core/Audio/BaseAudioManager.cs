using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LumosLib
{
    public abstract class BaseAudioManager : MonoBehaviour, IPreInitializer, IAudioManager
    {
        #region >--------------------------------------------------- PROPERTIE


        public int PreInitOrder => (int)PreInitializeOrder.Audio;
        

        #endregion
        #region >--------------------------------------------------- FIELD
        
        
        [SerializeField] protected AudioPlayer _playerPrefab;
        protected readonly Dictionary<string, SoundAsset> _assetResources = new();
        protected AudioMixer _mixer;
        
        
        #endregion
        #region >--------------------------------------------------- UNITY


        protected virtual void Awake()
        {
            GlobalService.Register<IAudioManager>(this);
            
            DontDestroyOnLoad(gameObject);
        }
        
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public virtual IEnumerator InitAsync()
        {
            var resourceManager = GlobalService.GetInternal<IResourceManager>();
            var resources = resourceManager.LoadAll<SoundAsset>("");
            
            foreach (var resource in resources)
            {
                _assetResources[resource.name] = resource;
            }

            _mixer =  Project.Config.Mixer;
            
            yield break;
        }
        
        
        #endregion
        #region >--------------------------------------------------- SET
        
        
        public void SetVolume(string groupName, float volume)
        {
            _mixer.SetFloat(groupName, Mathf.Log10(volume) * 20f);
        }
        
        
        #endregion
        #region >--------------------------------------------------- PLAY
        
        
        public abstract void PlayBGM(int bgmType, string assetName);
        public abstract void PlaySFX(string assetName);
        
        
        #endregion
        #region >--------------------------------------------------- STOP
        
        
        public abstract void StopBGM(int bgmType);
        public abstract void StopSFXAll();
        public abstract void StopAll();
        
        
        #endregion
        #region >--------------------------------------------------- PAUSE
        
        
        public abstract void PauseBGM(int bgmType, bool enable);
        public abstract void PauseSFXAll(bool enable);
        public abstract void PauseAll(bool enable);
        
        
        #endregion
    }
}