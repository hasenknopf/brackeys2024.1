using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlantShop/Watering Can")]
public class WateringCanSO : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public int uses;
}
