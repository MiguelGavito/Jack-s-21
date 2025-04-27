using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    public static PassiveItemManager Instance;

    public List<PassiveItem> passiveItems = new List<PassiveItem>();

    private InventoryManager data;

    private void Awake()
    {
        // Asegurar Singleton si lo quieres
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Si quieres que sobreviva entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        data = InventoryManager.instance;
        // itemList = data.GetPlayerItems();  // Si necesitas obtener la lista de items
    }

    public void LoadPassiveItemFromInventory()
    {
        var data = InventoryManager.instance;
        if (data != null)
        {
            passiveItems = data.GetPlayerItems();
        }
        else
        {
            Debug.LogWarning("InventoryManager no esta inicializado.");
        }
    }

    public void AddPassiveItem(PassiveItem item)
    {
        passiveItems.Add(item);
        item.UseItem(GameManager.instance);
    }

    public int GetTotalBonus()
    {
        int total = 0;
        foreach (var item in passiveItems)
        {
            total += item.bonusValue;
        }
        return total;
    }
}
