using UnityEngine;

public class UnitHealth
{
    private int _currentMaxHealth;
    private int _currentHealth;

    public int Health
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
    public int MaxHealth
    {
        get { return _currentMaxHealth; }
        set { _currentMaxHealth = value; }
    }

    /// <summary>
    /// constructor assigns health to the system 
    /// </summary>
    /// <param name="maxHealth"></param>
    /// <param name="currentHealth"></param>
    public UnitHealth(int maxHealth, int currentHealth)
    {
        _currentMaxHealth = maxHealth;
        _currentHealth = currentHealth;
    }

    /// <summary>
    /// loses health
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }

    /// <summary>
    /// heals but if heal + CurrentHP > maxHp  then just sets you at Max Hp
    /// </summary>
    /// <param name="heal"></param>
    public void Heal(int heal)
    {
        if (_currentHealth + heal <= _currentMaxHealth)
        {
            _currentHealth += heal;
        }
        else
        {
            _currentHealth = _currentMaxHealth;
        }
    }
    
    
}
