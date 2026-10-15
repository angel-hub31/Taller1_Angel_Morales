using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Valor de la Moneda")]
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si el objeto que atraviesa la moneda tiene el Tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Suma los puntos al contador de la interfaz UI
            if (CoinUI.instance != null)
            {
                CoinUI.instance.AddCoin(coinValue);
            }

            // Destruye la moneda del mapa
            Destroy(gameObject);
        }
    }
}