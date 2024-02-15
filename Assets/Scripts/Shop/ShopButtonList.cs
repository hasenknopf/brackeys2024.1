using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonList : MonoBehaviour
{
    [SerializeField] private ShopPlantButton _plantButtonPrefab;
    [SerializeField] private ShopPotButton _potButtonPrefab;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private List<PlantSO> _plants;
    [SerializeField] private PotSO _pot;

    private void Start()
    {
        GetButtons();
    }

    private void GetButtons()
    {
        int itemCount = 1;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Instantiate(_potButtonPrefab, transform).SetInfo(_pot);

        foreach (var plant in _plants)
        {
            Instantiate(_plantButtonPrefab, transform).SetInfo(plant);
            itemCount++;
        }

        _scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemCount * (_plantButtonPrefab.GetComponent<RectTransform>().rect.height + _scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing));
    }

    public void AddPlantToShop(PlantSO plant)
    {
        _plants.Add(plant);
        Instantiate(_plantButtonPrefab, transform).SetInfo(plant);
        SetSize();
    }

    public void RemovePlantFromShop(PlantSO plant)
    {
        var plantButton = GetComponentsInChildren<ShopPlantButton>().FirstOrDefault(btn => btn.CurrentPlant == plant);
        if(plantButton) Destroy(plantButton);
        _plants.Remove(plant);
        SetSize();
    }

    private void SetSize()
    {
        _scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_plants.Count+1) * (_plantButtonPrefab.GetComponent<RectTransform>().rect.height + _scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing));
    }
}
