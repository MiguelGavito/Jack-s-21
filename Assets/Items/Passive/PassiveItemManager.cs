using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{

    public List<PassiveItem> passiveItems = new List<PassiveItem>();

    public static PassiveItemManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
