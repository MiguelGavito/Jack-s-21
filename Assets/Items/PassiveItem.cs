using UnityEngine;

public class PassiveItem : Item
{
    public int bonusValue;  // Valor de la bonificaci�n que este objeto otorga

    // Este �tem podr�a tener efectos pasivos que se activan mientras est� en el inventario
    public override void UseItem(GameManager gameManager)
    {
        // Los objetos pasivos no suelen tener un "uso" directo, sino que tienen un efecto siempre activo
        Debug.Log($"{itemName} activado. El valor de bonificaci�n es {bonusValue}.");
        // Puedes a�adir l�gica para modificar caracter�sticas del jugador, como aumentar su puntaje de forma pasiva.
    }
}
