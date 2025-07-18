using UnityEngine;

public abstract class DamageHandler : ScriptableObject
{
    public abstract void TakeDamage(IDamageable target, int damageAmount, int resistance);
}

[CreateAssetMenu(menuName = "Damage Handlers/Simple Damage")]
public class SimpleDamageHandler : DamageHandler
{
    public override void TakeDamage(IDamageable target, int damageAmount, int resistance)
    {
        if (target.IsDamageable)
        {
            target.TakeDamage(damageAmount);
        }
    }
}

[CreateAssetMenu(menuName = "Damage Handlers/Resistance Damage")]
public class ResistanceDamageHandler : DamageHandler
{
    [SerializeField] private float resistanceMultiplier = 0.5f;
    
    public override void TakeDamage(IDamageable target, int damageAmount, int resistance)
    {
        if (target.IsDamageable)
        {
            var reducedDamage = Mathf.RoundToInt(damageAmount * resistanceMultiplier);
            target.TakeDamage(reducedDamage);
        }
    }
}

[CreateAssetMenu(menuName = "Damage Handlers/Invincibility Handler")]
public class InvincibilityHandler : DamageHandler
{
    public override void TakeDamage(IDamageable target, int damageAmount, int resistance)
    {
        Debug.Log("Target is invincible!");
    }
}