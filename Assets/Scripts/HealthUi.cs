using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour
{
    [Header("Referencia al Jugador")]
    public PlayerHealth playerHealth;

    [Header("Imágenes de Corazones (en orden de izq a der)")]
    public Image[] heartImages;

    void Update()
    {
        if (playerHealth == null) return;

        UpdateHeartsDisplay();
    }

    private void UpdateHeartsDisplay()
    {
        // Recorre las 3 imágenes de corazones
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                // Si el índice es menor que la vida actual, activa el corazón; si no, lo oculta
                heartImages[i].enabled = (i < playerHealth.currentLives);
            }
        }
    }
}