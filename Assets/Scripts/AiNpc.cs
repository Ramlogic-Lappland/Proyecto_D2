using UnityEngine;

public class AiNpc : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private bool _isDead = false;
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            _isDead = true;
        }
    }
}
