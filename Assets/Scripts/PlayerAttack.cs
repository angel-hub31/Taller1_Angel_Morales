
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Ajustes de ataque")]
    public Transform AttackPoint;
    public float attackRange=0.6f;
    public LayerMask enemyLayer;
    public int attackDamage=1;


    private Animator animator;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)|| Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        Collider2D[] hitEnemis=Physics2D.OverlapCircleAll(AttackPoint.position,attackRange,enemyLayer);
        foreach(Collider2D enemy in hitEnemis)
        {
            EnemyHealth enemyHealth=enemy.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position,attackRange);
            
        
    }
}
