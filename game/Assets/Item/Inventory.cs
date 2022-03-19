using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> items;

    public Inventory()
    {
        items = new List<Item>();
        AddItem(new Item{ amount=1, ItemType=Item.itemType.weapon});
        AddItem(new Item{ amount=1, ItemType=Item.itemType.armor});
        AddItem(new Item{ amount=1, ItemType=Item.itemType.healthPotion});
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }
       

    public List<Item> GetItemList()
    {
        return items;
    }
}   
