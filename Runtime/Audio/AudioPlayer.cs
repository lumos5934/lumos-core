using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LumosLib
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IPoolable 
    {
        private const float DefaultVolume = 1;
        private UnityAction<AudioPlayer> _onStop;
        private Coroutine _stopAsync;
        private AudioSource _audioSource;
        private bool _isPause;
        
        public float Volume => _audioSource.volume;
        public bool Loop => _audioSource.loop;
        public bool IsPlaying => _audioSource.isPlaying;
        public AudioClip Clip => _audioSource.clip;

        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();   
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }
        
        public void Play(SoundAsset asset, UnityAction<AudioPlayer> onStop)
        {
            _isPause = false;
            
            _audioSource.outputAudioMixerGroup = asset.MixerGroup;
            _audioSource.clip = asset.Clip;
            _audioSource.volume = DefaultVolume + asset.VolumeFactor;
            _audioSource.loop = asset.IsLoop;
            _audioSource.Play();

            _onStop = onStop;

            _stopAsync = StartCoroutine(StopAsync());
        }
        
        
        public void Stop()
        {
            _isPause = false;
            
            _audioSource.Stop();
            
            _onStop?.Invoke(this);
        }

        public void Pause(bool enable)
        {
            _isPause = enable;
            
            if (enable)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.UnPause();
            }
        }
        public void OnCreated()
        {
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            if (_stopAsync != null)
            {
                StopCoroutine(_stopAsync);

                _stopAsync = null;
            }
        }

        public void OnDestroyed()
        {
        }

        private IEnumerator StopAsync()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying || _isPause);
            
            Stop();
        }
    }
}