using UnityEngine;

public abstract class DamageHandler : ScriptableObject
{
    public abstract void ProcessDamage(IDamageable target, int baseDamage);
}

[CreateAssetMenu(menuName = "Damage Handlers/Simple Damage")]
public class SimpleDamageHandler : DamageHandler
{
    public override void ProcessDamage(IDamageable target, int baseDamage)
    {
        if (target.IsDamageable)
        {
            target.TakeDamage(baseDamage);
        }
    }
}

[CreateAssetMenu(menuName = "Damage Handlers/Resistance Damage")]
public class ResistanceDamageHandler : DamageHandler
{
    [SerializeField] private float resistanceMultiplier = 0.5f;
    
    public override void ProcessDamage(IDamageable target, int baseDamage)
    {
        if (target.IsDamageable)
        {
            var reducedDamage = Mathf.RoundToInt(baseDamage * resistanceMultiplier);
            target.TakeDamage(reducedDamage);
        }
    }
}

[CreateAssetMenu(menuName = "Damage Handlers/Invincibility Handler")]
public class InvincibilityHandler : DamageHandler
{
    public override void ProcessDamage(IDamageable target, int baseDamage)
    {
        Debug.Log("Target is invincible!");
    }
}