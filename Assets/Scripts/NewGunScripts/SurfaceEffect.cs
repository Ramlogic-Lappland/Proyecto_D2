using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceEffect", menuName = "Scriptable Objects/SurfaceEffect")]
public class SurfaceEffect : ScriptableObject
{
    public List<SpawnObjectEffect> SpawnEffects = new List<SpawnObjectEffect>();
    public List<PlayAudioEffect> PlayAudioEffects = new List<PlayAudioEffect>();
}
