using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Lumos.DevPack
{
    public class AudioManager : MonoBehaviour, IPreInitializer
    {
        #region >--------------------------------------------------- PROPERTIES


        public int Order => (int)PreInitializeOrder.Audio;
        public bool IsInitialized { get; private set; }
        
        
        #endregion
        
        #region >--------------------------------------------------- FIELDS
        
        
        private AudioMixer _mixer;
        private AudioPlayer _playerPrefab;
        private PoolManager _poolManager;
        private HashSet<AudioPlayer> _activePlayers = new();
        private Dictionary<int, AudioPlayer> _bgmPlayers = new();
        private Dictionary<int, AudioAssetSO> _assetResources = new();
        
        
        #endregion
        
        #region >--------------------------------------------------- INIT


        public Task InitAsync()
        {
            _poolManager = Global.Get<PoolManager>();
            
            var resourceMgr = Global.Get<ResourceManager>();
            var resources = resourceMgr.LoadAll<AudioAssetSO>(Path.Audio);

            foreach (var resource in resources)
            {
                _assetResources[resource.GetID()] = resource;
            }
            
            _playerPrefab = resourceMgr.Load<AudioPlayer>(Path.AudioPlayerPrefab);

            if (_playerPrefab == null)
            {
                DebugUtil.LogError(" wrong audio player path ", " INIT FAIL ");
                return null;
            }
            
            IsInitialized = true;
            return Task.CompletedTask;
        }
        

        #endregion

        #region >--------------------------------------------------- SET


        public void SetMixer(AudioMixer mixer)
        {
            _mixer = mixer;
        }
        
        public void SetVolume(string groupName, float volume)
        {
            _mixer.SetFloat(groupName, Mathf.Log10(volume) * 20f);
        }
        
        
        #endregion
        
        #region >--------------------------------------------------- PLAY

        
        public void PlayBGM(int bgmType, int assetId)
        {
            if (_bgmPlayers.TryGetValue(bgmType, out var containsPlayer))
            {
                Play(assetId, true, containsPlayer);
                return;
            }
            
            var bgmPlayer = _poolManager.Get(_playerPrefab);
            
            _bgmPlayers[bgmType] = bgmPlayer;
            
            Play(assetId, true, bgmPlayer);
        }
        
        public void PlaySFX(int assetId)
        {
            Play(assetId, false, _poolManager.Get(_playerPrefab));
        }
        
        private void Play(int assetId, bool isLoop, AudioPlayer player)
        {
            if (_assetResources.TryGetValue(assetId, out AudioAssetSO asset))
            {
                player.Play(asset, isLoop);
                
                _activePlayers.Add(player);
            }
        }

        
        #endregion
        
        #region >--------------------------------------------------- STOP


        public void StopBGM(int bgmType)
        {
            if (_bgmPlayers.TryGetValue(bgmType, out var containsPlayer))
            {
                containsPlayer.Stop();
            }
        }
        
        public void StopAll()
        {
            foreach (var player in _activePlayers)
            {
                player.Stop();
            }
        }

        
        #endregion
        
        #region >--------------------------------------------------- PUASE

        
        public void PauseBGM(int bgmType, bool enable)
        {
            if (_bgmPlayers.TryGetValue(bgmType, out var containsPlayer))
            {
                containsPlayer.Pause(enable);
            }
        }
        
        public void PauseAll(bool enable)
        {
            foreach (var player in _activePlayers)
            {
                player.Pause(enable);
            }
        }
        
        
        #endregion
        
        #region >--------------------------------------------------- POOL

        
        public void ReleaseToManager(AudioPlayer player)
        {
            _poolManager.Release(player);

            if (_activePlayers.Contains(player))
            {
                _activePlayers.Remove(player);
            }
        }
        
        
        #endregion
    }
}