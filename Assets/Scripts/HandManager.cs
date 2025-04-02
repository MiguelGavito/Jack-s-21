using TMPro;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // UI donde mostrarás el valor total

    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        int totalValue = 0;

        // Recorremos todas las cartas hijas del objeto padre
        foreach (Transform child in transform)
        {
            Card card = child.GetComponent<Card>();
            if (card != null)
            {
                totalValue += card.numero; // Asume que la carta tiene un campo `value`
            }
        }

        // Mostrar en UI
        scoreText.text = "Total: " + totalValue.ToString();
    }

}
