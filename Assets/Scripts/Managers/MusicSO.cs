using UnityEngine;

[CreateAssetMenu(fileName = "MusicSO", menuName = "Audio/Music Tracks")]
public class MusicSO : ScriptableObject
{
    public MusicTrack[] tracks;
}
