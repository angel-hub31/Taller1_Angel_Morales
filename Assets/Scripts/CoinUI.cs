using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public static CoinUI instance;

    [Header("Texto de Monedas")]
    public TextMeshProUGUI coinText;

    private int coinCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Monedas: " + coinCount;
        }
    }
}