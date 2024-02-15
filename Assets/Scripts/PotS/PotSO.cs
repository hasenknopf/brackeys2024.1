using UnityEngine;

[CreateAssetMenu(menuName = "PlantShop/Pot")]
public class PotSO : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public int maxPots;
    public Sprite potSprite;
}
