using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }
    private void Start()
    {
        Instance = this;
    }

    public Sprite weaponSprite;
    public Sprite armorSprite;
    public Sprite manaPotionSprite;
    public Sprite healthPotionSprite;
}
