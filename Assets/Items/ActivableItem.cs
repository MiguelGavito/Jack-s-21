using UnityEngine;

public class ActivableItem : Item
{
    public int charges; // Numero de cargas disponibles

    public override void UseItem(GameManager gameManager)
    {
        if (charges > 0)
        {
            // Logica para activar el objeto
            Debug.Log($"Usando el objeto activable: {itemName}. Aumentando puntuacion temporalmente");
            gameManager.puntaje += 10; // ejemplo, ahora no tiene sentido, cambiar por multiplicador de valor de la mano imaginaria;

            charges--; //Reducir las cargas
            if (charges <= 0)
            {
                isUsed |= true;
                Debug.Log($"{itemName} se ha agotado.");
            }
        }
        else
        {
            Debug.Log($"No tienes mas cargas de {itemName}.");
        }
    }
}