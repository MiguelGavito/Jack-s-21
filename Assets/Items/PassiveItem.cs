using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PassiveItem : Item
{
    public int bonusValue;  // Valor de la bonificación que este objeto otorga
    public List<ScriptableObject> effectObjects;

    // Este ítem podría tener efectos pasivos que se activan mientras está en el inventario
    public override void UseItem(GameManager gameManager)
    {
        // Los objetos pasivos no suelen tener un "uso" directo, sino que tienen un efecto siempre activo
        Debug.Log($"{itemName} activado. El valor de bonificación es {bonusValue}.");

        foreach (var obj in effectObjects)
        {
            if (obj is IPassiveEffect effect)
            {
                effect.Apply(gameManager);
            }
        }
    }
}
