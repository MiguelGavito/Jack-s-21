using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private List<Item> playerItems = new List<Item>();  // Aquí guardamos los objetos del jugador

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Mantener este objeto entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Añadir objeto al inventario
    public void AddItem(Item item)
    {
        playerItems.Add(item);
    }

    // Obtener los objetos del jugador
    public List<Item> GetPlayerItems()
    {
        return playerItems;
    }

    public void RemoveItem(Item item)
    {
        playerItems.Remove(item);
    }
}
