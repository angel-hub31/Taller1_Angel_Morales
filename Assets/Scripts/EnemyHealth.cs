using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header ("Salud del Enemigo")]
    public int MaxHealth=3;
    public int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth=MaxHealth;
        
    }

    public void TakeDamage(int damegeAmout)
    {
        currentHealth -= damegeAmout;
        if (currentHealth <=0)
        {
            Die();
        }
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    
}
