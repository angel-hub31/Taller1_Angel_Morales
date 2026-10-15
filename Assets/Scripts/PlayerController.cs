using UnityEngine; // libreria base de unity

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd;
    public float moveSpeed = 5f; // variable velocidad de movimiento
    public float jumpForce = 7f; // variable fuerza de salto
    private bool isGrounded; // variable booleana para verificar si esta en el suelo
    private Animator animator;

    private bool facinRight = true;
    private bool isAttacking = false;

    // Controla si el jugador está recibiendo empuje por daño para no sobreescribir la física
    [HideInInspector] public bool isKnocked = false;

    void Start()
    {
        // busca y asigna automaticamente el componente fisico Rigidbody2D
        rd = GetComponent<Rigidbody2D>();
        // busca y asigna el componente Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 0. Si está recibiendo empuje por daño, se ignoran las teclas y no se frena el impulso
        if (isKnocked) return;

        // Deteccion de bloque y ataque
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }

        if (isAttacking)
        {
            rd.linearVelocity = new Vector2(0f, rd.linearVelocity.y);
            return;
        }

        // 1. Capturamos el movimiento horizontal (-1, 0, 1)
        float move = Input.GetAxis("Horizontal");

        // 2. Controlamos la animacion según el suelo
        if (isGrounded)
        {
            float speedAnimation = Mathf.Abs(move);
            animator.SetFloat("Speed", speedAnimation);
        }
        else
        {
            animator.SetFloat("Speed", 0f); // En el aire no reproduce animación de correr
        }

        // 3. Aplicamos velocidad física horizontal conservando la velocidad vertical actual
        rd.linearVelocity = new Vector2(move * moveSpeed, rd.linearVelocity.y);

        // Voltear sprite segun la direccion
        if (move > 0 && !facinRight)
        {
            Flip();
        }
        else if (move < 0 && facinRight)
        {
            Flip();
        }

        // 4. Salto: detecta espacio si esta en el suelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJump", true);
        }
    }

    // Detecta o comprueba si el objeto con el que choco tiene etiqueta Ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Solo activa el suelo si el golpe viene desde abajo (los pies)
            if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.7f)
            {
                isGrounded = true;
                animator.SetBool("isJump", false);
            }
        }
    }

    // Detecta cuando el personaje deja de tocar el objeto "Ground"
    void OnCollisionExit2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isJump", true);
        }
    }

    void Flip()
    {
        facinRight = !facinRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Invoke(nameof(ResetAttack), 0.35f);
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }
}