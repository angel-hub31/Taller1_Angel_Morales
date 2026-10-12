

using UnityEngine; //libreria de unity

public class CameraFollow : MonoBehaviour //definimos la clase publica que hereda de Monobeaviour
{


    public Transform target; // guarda la referencia a la posicion, rotacion y escala edl objeto que la camara va a seguir
    public float smoothSpeed=5f;//define  que tan rapido y suave responde la camara al perseguir el objetivo
    public Vector3 offset= new Vector3(0,2,-10);//definimos la distancia de separacion entre camara y objeto en los 3 ejes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

//hace que la camara persiga suavemente al jugador sin dar tirones bruscos
    void LateUpdate()
    {
        if(target != null)
        {
            //cada frame espera a que el jugador termine de moverse, calcula donde
            //deberi de colocarse la camara manteniendo su distancia
            Vector3 desiredPosition=target.position + offset;

            Vector3 smoothedPosition=Vector3.Lerp(transform.position, desiredPosition,smoothSpeed * Time.deltaTime);

            transform.position= smoothedPosition;

        }
    }
}
