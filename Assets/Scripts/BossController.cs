using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Configuración de Salud")]
    public int maxHealth = 10;
    public Slider barraVidaUI;
    public GameObject panelVictoriaUI;
    private int currentHealth;

    [Header("Movimiento y Persecución")]
    public float speed = 3f;
    public Transform player;
    public float stoppingDistanceX = 0.5f;

    [Header("Detección del Jugador")]
    public float playerDetectRange = 8f;     // Distancia horizontal para detectar al jugador
    public float maxPlayerHeightDiff = 1.8f;  // Altura máxima accesible (altura de la plataforma)

    [Header("Detección de Obstáculos y Patrulla")]
    public Transform obstacleCheck;
    public float obstacleCheckDistance = 0.8f;
    public LayerMask obstacleLayer;

    // Estado de patrulla
    private bool isPatrolling = false;

    [Header("Fase de Furia")]
    public Color rageColor = Color.red;
    private bool isEnraged = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;

        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = maxHealth;
            barraVidaUI.value = currentHealth;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            float diffX = player.position.x - rb.position.x;
            float diffY = player.position.y - rb.position.y;
            float distanceToPlayer = Mathf.Abs(diffX);

            bool playerIsHigh = diffY > maxPlayerHeightDiff;
            bool playerInRange = distanceToPlayer <= playerDetectRange && !playerIsHigh;

            // 1. Si el jugador está arriba en la plataforma y el Boss está cerca debajo:
            // Se resigna, activa patrulla y sigue caminando hacia adelante
            if (playerIsHigh && distanceToPlayer < 2.0f)
            {
                if (!isPatrolling)
                {
                    isPatrolling = true;
                    // Mantiene la dirección previa o avanza si estaba detenido
                    if (moveDirection == 0) moveDirection = 1f;
                }
            }
            // 2. Si el jugador baja o está a una altura alcanzable:
            else if (playerInRange)
            {
                isPatrolling = false;
            }

            // 3. Comportamiento según el estado:
            if (!isPatrolling)
            {
                // Modo Persecución
                if (distanceToPlayer > stoppingDistanceX)
                {
                    moveDirection = Mathf.Sign(diffX);
                }
                else
                {
                    moveDirection = 0f;
                }
            }
            else
            {
                // Modo Patrulla: si en algún momento se detuvo, fuerza avance continuo
                if (moveDirection == 0)
                {
                    moveDirection = 1f;
                }
            }
        }

        // 4. Comprobar obstáculo de frente con Raycast
        if (moveDirection != 0 && CheckObstacleAhead())
        {
            HandleObstacleCollision(-moveDirection);
        }

        // 5. Volteo del sprite según la marcha
        if (spriteRenderer != null && moveDirection != 0)
        {
            spriteRenderer.flipX = (moveDirection < 0);
        }

        // 6. Aplicar movimiento
        Vector2 targetPosition = new Vector2(
            rb.position.x + moveDirection * speed * Time.deltaTime,
            rb.position.y
        );

        rb.MovePosition(targetPosition);
    }

    private void HandleObstacleCollision(float newDirection)
    {
        moveDirection = newDirection;

        // Si choca contra un muro patrullando, invierte la marcha para ir hacia el otro muro
        if (isPatrolling && player != null)
        {
            float diffY = player.position.y - rb.position.y;
            // Si el jugador ya bajó del escalón, cancela la patrulla
            if (diffY <= maxPlayerHeightDiff)
            {
                isPatrolling = false;
            }
        }
        else
        {
            isPatrolling = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                float awayFromWall = Mathf.Sign(contact.normal.x);
                if (Mathf.Sign(moveDirection) != awayFromWall)
                {
                    HandleObstacleCollision(awayFromWall);
                    break;
                }
            }
        }
    }

    private bool CheckObstacleAhead()
    {
        Vector2 origin = obstacleCheck != null ? (Vector2)obstacleCheck.position : (Vector2)transform.position;
        Vector2 direction = new Vector2(moveDirection, 0f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, obstacleCheckDistance, obstacleLayer);
        return hit.collider != null;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        isPatrolling = false;
        if (player != null)
        {
            float diffX = player.position.x - rb.position.x;
            if (Mathf.Abs(diffX) > 0.1f)
            {
                moveDirection = Mathf.Sign(diffX);
            }
        }

        if (barraVidaUI != null)
        {
            barraVidaUI.value = currentHealth;
        }

        if (!isEnraged && currentHealth <= (maxHealth / 2))
        {
            EnterRageMode();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void EnterRageMode()
    {
        isEnraged = true;
        speed *= 1.8f;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = rageColor;
        }
    }

    private void Die()
    {
        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(true);
        }

        Time.timeScale = 0f;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = obstacleCheck != null ? (Vector2)obstacleCheck.position : (Vector2)transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + new Vector2(moveDirection != 0 ? moveDirection : 1f, 0f) * obstacleCheckDistance);
    }
}