using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Impact System/Surface", fileName = "Surface")]
public class Surface : ScriptableObject
{
    [SerializeField]
    public class SurfaceImpactTypeEffect
    {
        public ImpactType impactType;
        public SurfaceEffect surfaceEffect;
    }
    public List<SurfaceImpactTypeEffect> ImpactTypeEffects = new List<SurfaceImpactTypeEffect>();
}
