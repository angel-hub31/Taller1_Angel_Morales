

using UnityEngine; //libreria de base unity

public class PlayerController : MonoBehaviour  // aqui se hereda de la clase MonoBehaviur
{                                              // los 2 puntos  ":" significa heredar

    private Rigidbody2D rd;
    public float moveSpeed= 5f;// variable velocidad de movimiento
    public float jumpForce= 7f;//variable fuerza de salto
    private bool isGrounded; //variable booleana para verificar si esta en el suelo (isGrounded)
    private Animator animator;

    private bool facinRight= true;

    private bool isAttacking= false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()// se ejecuta esta funcion la primera vez que lanza el videojuego y se ejecuta este metodo
    {            // se inicializa una sola vez en el juego

    //busca y asigna automaticamente el componente fisico rigidbidy2D
        rd = GetComponent <Rigidbody2D>();
        //busca y asigna para enviar parametros de animacion mas adelante
        animator=GetComponent<Animator>();
        
    }
    // Update is called once per frame
    void Update()// ejecuta a nivel de frames; se ejecuta en bucles todas las veces que se esté jugando
    {             // se ejecuta por fps

//deteccion de bloque y ataque
    if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
        if (isAttacking)
        {
            rd.linearVelocity= new Vector2(0f, rd.linearVelocity.y);
            return;
        }

//  Primero capturamos el movimiento y lee las teclas horizontal izquierda o derecha devuelva valores -1,0,1
        float move = Input.GetAxis("Horizontal");

        // 2. Controlamos la animación según el suelo
        if (isGrounded)
        {
            float speedAnimation = Mathf.Abs(move);
            animator.SetFloat("Speed", speedAnimation);
        }
        else
        {
            animator.SetFloat("Speed", 0f); // En el aire no reproduce animación de correr
        }

        // 3. Aplicamos velocidad física horizontal multiplicando la direccion "move" por la velocidad deseada (moveSpeed)
        rd.linearVelocity = new Vector2(move * moveSpeed, rd.linearVelocity.y);

        if (move > 0 && !facinRight)
        {
            Flip();
        }
        else if (move < 0 && facinRight)
        {
            Flip();
        }

        // salto: detecta el boton space  de si se presiono y si está en el suelo
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded= false;
            animator.SetBool("isJump", true);
        }
    }
    //detecta o comprueba si el objeto con el que choco tiene etiqueta Ground
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
    //detecta cuando el personaje deja de tocar el objeto "Ground"
    void OnCollisionExit2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Ground"))
        {
            isGrounded= false;
            animator.SetBool("isJump",true);
        }
    }

    void Flip()
    {
        facinRight=!facinRight;
        Vector3 scale = transform.localScale;
        scale.x *=-1;
        transform.localScale=scale;
    }

    //activa el bloque de movimiento y dispara el disparador Trigger en el animator para ejecutarla animacion de golpe o ataque
    void Attack()
    {
        isAttacking=true;
        animator.SetTrigger("Attack");

        Invoke(nameof(ResetAttack),0.35f);
    }
    //vuelve a colocar el ataque en false permitiendo que el jugador vuelva a moverse, saltar o atacar de nuevo
    public void ResetAttack()
    {
        isAttacking = false;
    }

    
}
