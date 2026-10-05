using UnityEngine;

public class PlayerController : MonoBehaviour  // aqui se hereda de la clase MonoBehaviur
{                                              // los 2 puntos  ":" significa heredar

    private Rigidbody2D rd;
    public float moveSpeed= 5f;// variable velocidad de movimiento
    public float jumpForce= 7f;//variable fuerza de salto
    private bool isGrounded; //variable booleana para verificar si esta en el suelo (isGrounded)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()// se ejecuta esta funcion la primera vez que lanza el videojuego y se ejecuta este metodo
    {            // se inicializa una sola vez en el juego
        rd = GetComponent <Rigidbody2D>();
        
    }
    // Update is called once per frame
    void Update()// ejecuta a nivel de frames; se ejecuta en bucles todas las veces que se esté jugando
    {             // se ejecuta por fps

        float move = Input.GetAxis("Horizontal");

        rd.linearVelocity = new Vector2 (move * moveSpeed , rd.linearVelocity.y);

        // salto: detecta el boton space  de si se presiono y si está en el suelo
        if(Input.GetButtonDown("Jump") && isGrounded){
            rd.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded= false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            isGrounded= true;
        }
    }
    void OnCollisionExit2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Ground"))
        {
            isGrounded= false;
        }
    }
}
