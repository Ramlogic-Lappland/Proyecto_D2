using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Impact System/PLayAudioEffect", fileName = "PlayAudioEffect")]
public class PlayAudioEffect : ScriptableObject
{
    public AudioSource audioSourcePrefab;
    public List<AudioClip> audioClips = new List<AudioClip>();

    [Tooltip("values are clamped to 0 & 1")]
    public Vector2 volumeRange = new Vector2(0, 1);
}
