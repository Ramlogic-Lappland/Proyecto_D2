using UnityEditor.XR;
using UnityEngine;
public  interface IDamageable
{
    void TakeDamage(int damage);
    bool IsDamageable { get; }
    int CurrentHealth { get; }
}
[System.Serializable]
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private bool isDamageable = true;

    public bool IsDamageable => isDamageable;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public event System.Action OnHealthChanged;
    public event System.Action OnDeath;
    
    public void Initialize(int maxHP)
    {
        maxHealth = maxHP;
        currentHealth = maxHP;
        isDamageable = true;
        OnHealthChanged?.Invoke();
    }
    
    public void TakeDamage(int damage)
    {
        if (!IsDamageable) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke();

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke();
    }

    public void SetDamageable(bool canTakeDamage)
    {
        isDamageable = canTakeDamage;
    }
}
