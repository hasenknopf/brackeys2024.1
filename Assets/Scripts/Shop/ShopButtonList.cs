using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonList : MonoBehaviour
{
    [SerializeField] float offset = 20f;
    [SerializeField] private ShopPlantButton _plantButtonPrefab;
    [SerializeField] private ShopPotButton _potButtonPrefab;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private List<PlantSO> _plants;
    [SerializeField] private List<PotSO> _pots;

    private void Start()
    {
        GetButtons();
    }

    private void GetButtons()
    {
        int plantCount = 0;
        int potCount = 0;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var pot in _pots)
        {
            Instantiate(_potButtonPrefab, transform).SetInfo(pot);
            potCount++;
        }

        foreach (var plant in _plants)
        {
            Instantiate(_plantButtonPrefab, transform).SetInfo(plant);
            plantCount++;
        }

        _scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((plantCount * _plantButtonPrefab.GetComponent<RectTransform>().rect.height) + (potCount * _potButtonPrefab.GetComponent<RectTransform>().rect.height) + _scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing) + offset);
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
        _scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_plants.Count * _plantButtonPrefab.GetComponent<RectTransform>().rect.height) +(_pots.Count * _potButtonPrefab.GetComponent<RectTransform>().rect.height) + _scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing + offset);
    }
}
