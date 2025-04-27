using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public GameManager gameManager;
    public GameObject passiveItemPrefab;
    public ScriptableObject extraCardLimit;

    public Transform passiveItemParent;


    public List<PassiveItem> playerItems = new List<PassiveItem>();  // Aquí guardamos los objetos del jugador

    public int playerGems = 10;
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

    public void AddPassiveItem(IPassiveEffect effect)
    {
        // Crear una nueva instancia del prefab
        GameObject newPassiveItemObject = Instantiate(passiveItemPrefab);

        // Obtener el script del PassiveItem
        PassiveItem passiveItem = newPassiveItemObject.GetComponent<PassiveItem>();


        // Ahora puedes asignar este PassiveItem al display
        PassiveItemDisplay display = newPassiveItemObject.GetComponent<PassiveItemDisplay>();
        display.SetItem(passiveItem);

        // Si tienes un GameManager y quieres aplicar el efecto
        passiveItem.UseItem(gameManager);
    }

    public void ResetInventory()
    {
        playerItems = new List<PassiveItem>();  // Aquí guardamos los objetos del jugador
        playerGems = 10;
        round = 1;
    }

    // Añadir objeto al inventario
    public void AddItem(PassiveItem item)
    {
        playerItems.Add(item);
        item.ApplyEffect(GameManager.instance); // Aplica el efecto pasivo
    }

    // Obtener los objetos del jugador
    public List<PassiveItem> GetPlayerItems()
    {
        return playerItems;
    }

    public void RemoveItem(PassiveItem item)
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
