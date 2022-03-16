using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public enum itemType 
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
    public itemType ItemType;
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

}
