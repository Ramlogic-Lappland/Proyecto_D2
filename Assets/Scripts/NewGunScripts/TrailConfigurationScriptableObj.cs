using UnityEngine;

[CreateAssetMenu(fileName = "TrailConfigurationScriptableObj", menuName = "Guns/TrailConfigurationScriptableObj", order = 4)]
public class TrailConfigurationScriptableObj : ScriptableObject
{
    public Material material;
    public AnimationCurve widthCurve;
    public float duration;
    public float minVertexDistance = 0.1f;
    public Gradient color;
    
    public float missDistance = 100f;
    public float simulationSpeed = 150f;
}
