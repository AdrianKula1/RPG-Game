using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType 
    { 
        weapon,
        armor,
        healthPotion,
        manaPotion
    }


    public string name;
    public string discription;
    public int capacity;
    public List<ItemStatistic> stat;
    public ItemType itemType;
    public int amount;

    public string GetName()
    {
        return name;
    }
    public void SetName(string x) 
    {
        name = x;
    }
    public string GetDescroption()
    {
        return discription;
    }
    public void SetDescroption(string x)
    {
       discription = x;
    }

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.weapon:           return ItemAssets.Instance.weaponSprite;
            case ItemType.armor:            return ItemAssets.Instance.armorSprite;
            case ItemType.healthPotion:     return ItemAssets.Instance.healthPotionSprite;
            case ItemType.manaPotion:       return ItemAssets.Instance.manaPotionSprite;
        }
    }
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.healthPotion:
            case ItemType.manaPotion:      
                return true;
            case ItemType.weapon:
            case ItemType.armor:
                return false;
        }
    }
}



