using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{

    public List<PassiveItem> passiveItems = new List<PassiveItem>();

    public List<Item> itemList = new List<Item>();

    public static PassiveItemManager Instance;

    InventoryManager data = InventoryManager.instance;

    private void Start()
    {
        itemList = data.GetPlayerItems();
    }
    public void AddPassiveItem(PassiveItem item)
    {
        passiveItems.Add(item);
        item.UseItem(GameManager.instance); // activa efecto pasivo
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
