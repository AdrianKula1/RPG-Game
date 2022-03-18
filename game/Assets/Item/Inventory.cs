using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> items;

    public Inventory()
    {
        items = new List<Item>();
        AddItem(new Item() { amount=1, ItemType=Item.itemType.weapon});
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

}   
