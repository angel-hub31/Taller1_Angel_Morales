using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Invulnerabilidad")]
    public float invulnerabilityDuration = 1.0f;
    private bool isInvulnerable = false;

    [Header("Empuje (Knockback)")]
    public Vector2 knockbackForce = new Vector2(5f, 4f);
    public float knockbackDuration = 0.25f; // Tiempo que el jugador pierde el control

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerController playerController;

    void Start()
    {
        currentLives = maxLives;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void TakeDamage(int damage, Transform enemyTransform)
    {
        currentLives -= damage;

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            // Determinar la dirección contraria al enemigo
            float direction = transform.position.x < enemyTransform.position.x ? -1f : 1f;
            Vector2 pushDir = new Vector2(direction * knockbackForce.x, knockbackForce.y);

            // Iniciar estados de empuje e invulnerabilidad
            StartCoroutine(ApplyKnockback(pushDir));
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator ApplyKnockback(Vector2 force)
    {
        if (playerController != null)
        {
            playerController.isKnocked = true; // Bloquea la sobreescritura de input
        }

        // Resetea la velocidad previa e impulsa con Unity 6
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        if (playerController != null)
        {
            playerController.isKnocked = false; // Devuelve el control al jugador
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    private void Die()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}