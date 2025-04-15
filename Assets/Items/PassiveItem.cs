using UnityEngine;

public class PassiveItem : Item
{
    public int bonusValue;  // Valor de la bonificación que este objeto otorga

    // Este ítem podría tener efectos pasivos que se activan mientras está en el inventario
    public override void UseItem(GameManager gameManager)
    {
        // Los objetos pasivos no suelen tener un "uso" directo, sino que tienen un efecto siempre activo
        Debug.Log($"{itemName} activado. El valor de bonificación es {bonusValue}.");
        // Puedes añadir lógica para modificar características del jugador, como aumentar su puntaje de forma pasiva.
    }
}
