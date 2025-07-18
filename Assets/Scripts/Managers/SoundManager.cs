using UnityEngine;

public enum SoundType
{
    PISTOL,
    SHOTGUN,
    RELOAD,
    WALK,
    HURT,
    DOOR,
    UICLICK,
    UIHOVER,
    ALERT,
    MUSIC1,
    MUSIC2
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;
    private static SoundManager Instance;
    /// <summary>
    /// declares that the instance of SoundManager is this one
    /// </summary>
    private void Awake() // Singleton
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType soundType, float volume = 1)
    {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)soundType], volume);
    }
}
