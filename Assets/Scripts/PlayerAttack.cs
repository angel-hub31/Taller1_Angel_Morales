using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ajustes de ataque")]
    public Transform AttackPoint;
    public float attackRange = 0.6f;
    public LayerMask enemyLayer;
    public int attackDamage = 1;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoGolpe);
            }
            Attack();
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Collider2D[] hitEnemis = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemis)
        {
            // Comprobación para enemigos comunes
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }

            // Comprobación para el Boss
            BossController bossController = enemy.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}