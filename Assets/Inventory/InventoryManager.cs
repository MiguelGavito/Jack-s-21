using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InventoryManager : MonoBehaviour
{
    [Header("Instancia")]
    public static InventoryManager instance;
    [Header("Referencias")]
    public GameManager gameManager;

    [Header("Cosas Inutiles")]
    public GameObject passiveItemPrefab;
    public ScriptableObject extraCardLimit;
    public Transform passiveItemParent;
    public List<PassiveItem> playerItems = new List<PassiveItem>();  // Aquí guardamos los objetos del jugador
    [Header("Datos del Jugador")]
    public int playerGems = 10;
    public int round = 1;
    [Header("Estadisticas de Juego")]
    public int limiteCart = 21;
    public int lives = 5;
    public float multiplicadorRecompensas = 1.0f;
    [Header("Contadores de mejoras")]
    public int mejorasVidasCompradas = 0;
    public int mejorasLimiteCompradas = 0;
    public int mejorasMultiplicadorCompradas = 0;

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

        playerGems = 20;
        round = 1;
        limiteCart = 21;
        lives = 5;
        multiplicadorRecompensas = 1.0f;

        mejorasVidasCompradas = 0;
        mejorasLimiteCompradas = 0;
        mejorasMultiplicadorCompradas = 0;
    }

    // Metodos para actualizar estadisticas
    public void AumentarVidas(int cantidad)
    {
        lives += cantidad;
        mejorasVidasCompradas++;
    }
    public void AumentarLimiteCartas(int cantidad)
    {
        limiteCart += cantidad;
        mejorasLimiteCompradas++;
    }
    public void AumentarMultiplicador(float cantidad)
    {
        multiplicadorRecompensas += cantidad;
        mejorasMultiplicadorCompradas++;
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
