using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(menuName ="PlantShop/Plant")]
public class PlantSO : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public Sprite seedSprite; 
    public Sprite grownSprite;
    public AnimatorController animatorController;
}
