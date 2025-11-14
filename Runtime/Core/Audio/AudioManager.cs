using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LumosLib
{
    public class AudioManager : BaseAudioManager
    {
        #region >--------------------------------------------------- FIELD
      
        
        private readonly Dictionary<int, AudioPlayer> _bgmPlayers = new();
        private readonly HashSet<AudioPlayer> _activePlayers = new();
        
        private IPoolManager _poolManager;
        
        
        #endregion
        #region >--------------------------------------------------- INIT

        
        public override IEnumerator InitAsync()
        {
           yield return base.InitAsync();
           
           _poolManager = GlobalService.GetInternal<IPoolManager>();
        }

        #endregion
        #region >--------------------------------------------------- GET & SET 
       

        private AudioPlayer GetBGMPlayer(int bgmType)
        {
            return _bgmPlayers.GetValueOrDefault(bgmType);
        }

        private AudioPlayer CreateNewPlayer()
        {
            var player = _poolManager.Get(_playerPrefab);
            
            _activePlayers.Add(player);
            
            return player;
        }


        #endregion
        #region >--------------------------------------------------- PLAY


        public override void PlayBGM(int bgmType, string assetName)
        {
            if (_assetResources.TryGetValue(assetName, out SoundAsset asset))
            {
                var bgmPlayer = GetBGMPlayer(bgmType) ?? CreateNewPlayer();

                _bgmPlayers.TryAdd(bgmType, bgmPlayer);
            
                bgmPlayer.Play(asset, _ =>
                {
                    OnStopBGM(bgmType, bgmPlayer);
                });
            }
        }

        public override void PlaySFX(string assetName)
        {
            if (_assetResources.TryGetValue(assetName, out SoundAsset asset))
            {
                CreateNewPlayer().Play(asset, OnStopSFX);
            }
        }
        
        
        #endregion
        #region >--------------------------------------------------- STOP


        public override void StopBGM(int bgmType)
        {
            GetBGMPlayer(bgmType)?.Stop();
        }
        
        public override void StopSFXAll()
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

        
        public override void StopAll()
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


        public override void PauseBGM(int bgmType, bool enable)
        {
            GetBGMPlayer(bgmType)?.Pause(enable);
        }
        
        public override void PauseSFXAll(bool enable)
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

        public override void PauseAll(bool enable)
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