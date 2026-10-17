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

        if (AudioManager.Instance != null && AudioManager.Instance.sonidoDanioEnemigo != null)
        {
            AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoDanioEnemigo);
        }
        
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
