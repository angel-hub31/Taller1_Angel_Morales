using UnityEngine;
using System.Collections;

public class TimedSpikeTrap : MonoBehaviour
{
    [Header("Ajustes de tiempo")]
    public float waitTime=2f;
    public float activeTime=1f;
    

    [Header("Ajustes de daño")]
    public int damageAmount=1;

    [Header("Estado de trampa")]
    public bool isDangerous=false;

    private Animator animator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TrapRoutine());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcesarDanio(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ProcesarDanio(other);
    }
//guarda el estado en el que se quedó nuestra rutina
    IEnumerator TrapRoutine()
    {
        while (true)
        {
            isDangerous=false;
            yield return new WaitForSeconds(waitTime);

            if (animator != null)
            {
                animator.SetTrigger("Active");
            }
            isDangerous= true;
            yield return new WaitForSeconds(activeTime);

            isDangerous=false;
        }
    }

    void ProcesarDanio(Collider2D other)
    {
        if (isDangerous)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount,transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
