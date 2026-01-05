using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace LumosLib
{
    public class AudioManager : MonoBehaviour, IPreInitializable, IAudioManager
    {
        #region >--------------------------------------------------- FIELD
      
      
        [SerializeField] private AudioPlayer _audioPlayerPrefab;
        [SerializeField] private AudioMixer _mixer;
        
        private readonly Dictionary<string, SoundAsset> _assetResources = new();
        private readonly Dictionary<int, AudioPlayer> _bgmPlayers = new();
        private readonly HashSet<AudioPlayer> _activePlayers = new();
        
        [Title("REQUIREMENT")]
        [ShowInInspector, HideReferencePicker, ReadOnly, LabelText("IResourceManager")] private IResourceManager _resourceManager;
        [ShowInInspector, HideReferencePicker, ReadOnly, LabelText("IPoolManager")] private IPoolManager _poolManager;
        
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            _resourceManager = GlobalService.Get<IResourceManager>();
            if (_resourceManager == null)
                return UniTask.FromResult(false);
            
            _poolManager = GlobalService.Get<IPoolManager>();
            if (_poolManager == null)
                return UniTask.FromResult(false);
            
            var soundResources = _resourceManager.LoadAll<SoundAsset>("");
            foreach (var resource in soundResources)
            {
                _assetResources[resource.name] = resource;
            }

            GlobalService.Register<IAudioManager>(this);
            return UniTask.FromResult(true);
        }
        
        
        #endregion
        #region >--------------------------------------------------- GET & SET 
       

        private AudioPlayer GetBGMPlayer(int bgmType)
        {
            return _bgmPlayers.GetValueOrDefault(bgmType);
        }

       
        private AudioPlayer GetNewAudioPlayer()
        {
            var player = _poolManager.Get(_audioPlayerPrefab);
            
            _activePlayers.Add(player);
            
            return player;
        }

        public void SetVolume(string groupName, float volume)
        {
            _mixer.SetFloat(groupName, Mathf.Log10(volume) * 20f);
        }

        
        #endregion
        #region >--------------------------------------------------- PLAY


        public void PlayBGM(int bgmType, string assetName)
        {
            if (_assetResources.TryGetValue(assetName, out SoundAsset asset))
            {
                var bgmPlayer = GetBGMPlayer(bgmType) ?? GetNewAudioPlayer();

                _bgmPlayers.TryAdd(bgmType, bgmPlayer);
            
                bgmPlayer.Play(asset, _ =>
                {
                    OnStopBGM(bgmType, bgmPlayer);
                });
            }
        }

        public void PlaySFX(string assetName)
        {
            if (_assetResources.TryGetValue(assetName, out SoundAsset asset))
            {
                GetNewAudioPlayer().Play(asset, OnStopSFX);
            }
        }
        
        
        #endregion
        #region >--------------------------------------------------- STOP


        public void StopBGM(int bgmType)
        {
            GetBGMPlayer(bgmType)?.Stop();
        }
        
        public void StopSFXAll()
        {
            var temp = new List<AudioPlayer>(_activePlayers);
            
            foreach (var player in temp)
            {
                if (!_bgmPlayers.ContainsValue(player))
                {
                    player.Stop();
                }
            }
        }

        
        public void StopAll()
        {
            var temp = new List<AudioPlayer>(_activePlayers);
            
            foreach (var player in temp)
            {
                player.Stop();
            }
        }

        private void OnStopBGM(int bgmType, AudioPlayer player)
        {
            _poolManager.Release(player);

            _bgmPlayers.Remove(bgmType);
            
            if (_activePlayers.Contains(player))
            {
                _activePlayers.Remove(player);
            }
        }
        
        private void OnStopSFX(AudioPlayer player)
        {
            _poolManager.Release(player);
            
            if (_activePlayers.Contains(player))
            {
                _activePlayers.Remove(player);
            }
        }


        #endregion
        #region >--------------------------------------------------- PAUSE


        public void PauseBGM(int bgmType, bool enable)
        {
            GetBGMPlayer(bgmType)?.Pause(enable);
        }
        
        public void PauseSFXAll(bool enable)
        {
            var temp = new List<AudioPlayer>(_activePlayers);
            
            foreach (var player in temp)
            {
                if (!_bgmPlayers.ContainsValue(player))
                {
                    player.Pause(enable);
                }
            }
        }

        public void PauseAll(bool enable)
        {
            var temp = new List<AudioPlayer>(_activePlayers);
            
            foreach (var player in temp)
            {
                player.Pause(enable);
            }
        }


        #endregion
    }
}