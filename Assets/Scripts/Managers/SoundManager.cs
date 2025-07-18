using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

  [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private SoundsSO soundEffectsSO;
        [SerializeField] private MusicSO musicTracksSO;
        
        [Header("Audio Sources")]
        [SerializeField] private AudioSource soundEffectsSource;
        [SerializeField] private AudioSource musicSource;

        private static SoundManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void InitializeAudioSources()
        {
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (soundEffectsSource == null) soundEffectsSource = gameObject.AddComponent<AudioSource>();
            
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            
            soundEffectsSource.playOnAwake = false;
        }

        
        public static void PlaySound(SoundType sound, float volumeModifier = 1f) // SOUND EFFECTS 
        {
            var soundList = _instance.soundEffectsSO.sounds[(int)sound];
            var clip = soundList.sounds[Random.Range(0, soundList.sounds.Length)];
            
            _instance.soundEffectsSource.outputAudioMixerGroup = soundList.mixer;
            _instance.soundEffectsSource.PlayOneShot(clip, soundList.volume * volumeModifier);
        }

       
        public static void PlayMusic(MusicType track, float fadeDuration = 1f)
        {
            if (_instance == null || _instance.musicSource == null) return;
    
            var musicTrack = _instance.musicTracksSO.tracks[(int)track];
    
            if (_instance.musicSource != null)
            {
                _instance.musicSource.outputAudioMixerGroup = musicTrack.mixer;
                _instance.musicSource.clip = musicTrack.clip;
                _instance.musicSource.volume = musicTrack.volume;
                _instance.musicSource.Play();
            }
        }

        public static void StopMusic(float fadeDuration = 1f)
        {
            _instance.musicSource.Stop();
        }

        public static void SetMusicVolume(float volume)
        {
            _instance.musicSource.volume = volume;
        }
    }

    [System.Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }

    [System.Serializable]
    public struct MusicTrack
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip clip;
    }