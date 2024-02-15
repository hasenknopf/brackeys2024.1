using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleSideMenu;

public class ShopPlantButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TMP_Text labelName;
    [SerializeField] TMP_Text labelPrice;
    [SerializeField] Image previewImage;
    [SerializeField] Plant plant;
    private PlantSO _currentPlant;
    public PlantSO CurrentPlant => _currentPlant;

    public void SetInfo(PlantSO plant)
    {
        labelName.text = plant.itemName;
        labelPrice.text = "Price: "+plant.itemPrice;
        previewImage.sprite = plant.grownSprite;
        _currentPlant = plant;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Instantiate(plant).SetInfo(_currentPlant);
        FindFirstObjectByType<SimpleSideMenu>().Close();
    }
}
