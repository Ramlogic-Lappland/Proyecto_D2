using UnityEngine;

[CreateAssetMenu( menuName = "Impact System/Spawn Object Effect",fileName = "SpawnObjectEffect" )]
public class SpawnObjectEffect : ScriptableObject
{
    public GameObject prefab;
    public float probability = 1;
    public bool randomizeRotation;
    [Tooltip("zero values will lock the rotation on that axis")] public Vector3 randomizedRotationMultiplier = Vector3.zero;
}
