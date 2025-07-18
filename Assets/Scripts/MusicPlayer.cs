using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private MusicType menuMusic = MusicType.MAIN_MENU;
    [SerializeField] private MusicType gameMusic = MusicType.LEVEL_1;
    [SerializeField] private MusicType bossMusic = MusicType.BOSS_FIGHT;
    [SerializeField] private float fadeDuration = 1f;

    private static MusicPlayer _instance;
    private AudioSource _musicSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSource();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudioSource()
    {
        _musicSource = GetComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;
        _musicSource.spatialBlend = 0;
    }

    private void OnSceneChanged(Scene previous, Scene current)
    {
        // Map scene names to music tracks
        switch (current.name)
        {
            case "MainMenu":
                SoundManager.PlayMusic(menuMusic, fadeDuration);
                break;
            case "Credits":
                break;
                
            case "Tutorial":
                SoundManager.PlayMusic(gameMusic, fadeDuration);
                break;
            case "Level01":
                SoundManager.PlayMusic(gameMusic, fadeDuration);
                break;
            case "Level02":
                SoundManager.PlayMusic(bossMusic, fadeDuration);
                break;
                
 
        }
    }
    void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }

    public void PlayDefaultTrack()
    {
        if (_musicSource != null)
        {
            SoundManager.PlayMusic(menuMusic, fadeDuration);
        }
    }
}