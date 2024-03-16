using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="PlantShop/Plant")]
public class PlantSO : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public int unlockCoins;
    public Sprite seedSprite; 
    public Sprite grownSprite;
    public RuntimeAnimatorController animatorController;
    public int daysToGrow;
    public int grownPrice;
    public int seedPrice;
    public int dryPrice = 1;
}
