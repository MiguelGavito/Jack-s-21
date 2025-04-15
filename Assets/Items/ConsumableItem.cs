using UnityEngine;

public class ConsumableItem : Item
{
    public override void UseItem(GameManager gameManager)
    {
        if (!isUsed)
        {
            Debug.Log($"Usando el objeto consumible: {itemName}. Aumentando la mano.");
            // Logica para mejorar la mano del jugador, por ejemplo, sumar puntos.
            gameManager.puntaje += 5;

            isUsed = true;
        }
        else
        {
            Debug.Log($"{itemName} ya ha sido usado.");
        }
    }
}