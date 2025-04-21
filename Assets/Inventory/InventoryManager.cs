using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;


    private List<Item> playerItems = new List<Item>();  // Aquí guardamos los objetos del jugador

    public int playerGems = 0;
    public int round = 1;

    public int PuntajeObjetivo => CalcularPuntajeObjetivo(round);

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

    public void ResetInventory()
    {
        playerItems = new List<Item>();  // Aquí guardamos los objetos del jugador
        playerGems = 0;
        round = 1;
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

    private int CalcularPuntajeObjetivo(int r)
    {
        int suma = 0;
        for (int i = 1; i <= r; i++)
        {
            suma += i * 100;
        }
        return suma;
    }

    public void AvanzarRound()
    {
        round++;
    }

    public void AgregarGemas(int cantidad)
    {
        playerGems += cantidad;
    }
}
