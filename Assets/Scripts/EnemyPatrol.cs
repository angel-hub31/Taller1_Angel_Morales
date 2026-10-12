
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Ajustes de movimiento")]

    public float speed=3f;
    private bool movinRight= true;

    [Header("Detectores")]

    public Transform groundCheck;
    public float distanceToGround=1.5f;
    public float distanceToWall=1.5f;
    public LayerMask grondLayer;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    //instanciamos rigidbody 2d
    {
        rb= GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //operador ternario: parecido a la sentencia if pero mas sencillo
        // se lee:si movinRight es true usa speed, si es falso usa -speed
        rb.linearVelocity=new Vector2(movinRight ? speed : -speed, rb.linearVelocity.y);
        Vector2 direction=movinRight ? Vector2.right : Vector2.left;

        RaycastHit2D  isGrondHead= Physics2D.Raycast(groundCheck.position,Vector2.down,distanceToGround,grondLayer);

        RaycastHit2D isWallsAhead= Physics2D.Raycast(transform.position,direction,distanceToWall,grondLayer);

        if(isGrondHead.collider==null || isWallsAhead.collider != null)
        {
            Flip();
        }


        
    }
    //creamos la funcion flip()
    void Flip()
    {
        movinRight=!movinRight;
        Vector3 scale= transform.localScale;
        scale.x *=-1;
        transform.localScale=scale;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(groundCheck != null)
        {
            Gizmos.DrawRay(groundCheck.position, Vector2.down * distanceToGround);
        }
        Vector2 direction=movinRight ? Vector2.right : Vector2.left;
        Gizmos.DrawRay(transform.position,direction * distanceToWall);
        
    }
}
