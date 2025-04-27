using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    InventoryManager data = InventoryManager.instance;

    

    public List<PassiveItem> PlayerItems;

    public int gems;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerItems = data.GetPlayerItems();
        gems = data.playerGems;
        
    }


    public void comprarItem1(PassiveItem item)
    {
        if( gems < 100)
        {
            gems-=100;
            data.AddItem(item);
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
