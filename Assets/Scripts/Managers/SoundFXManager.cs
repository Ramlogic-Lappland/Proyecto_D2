using System;
using Unity.Mathematics;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXObject;
/// <summary>
/// Declares a  singleton of this class
/// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates
        }
    }
/// <summary>
/// Instantiates a sound effect given 
/// </summary>
/// <param name="audioClip"></param>
/// <param name="spawnTransform"></param>
/// <param name="volume"></param>
    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        var audioSource = Instantiate(soundFXObject, spawnTransform.position, quaternion.identity);
        
        audioSource.clip = audioClip;
        
        audioSource.volume = volume;
        
        audioSource.Play();
        
        var clipLength = audioSource.clip.length;
        
        Destroy(audioSource.gameObject, clipLength);
    }
}
