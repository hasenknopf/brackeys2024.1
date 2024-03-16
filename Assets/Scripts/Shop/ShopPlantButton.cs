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
    [SerializeField] TMP_Text unlockCoinsRequirement;
    [SerializeField] GameObject lockScreen;
    [SerializeField] Image previewImage;
    [SerializeField] Plant plant;
    private int _unlockCoins;
    private PlantSO _currentPlant;
    public PlantSO CurrentPlant => _currentPlant;

    [HideInInspector] public bool plantButtonIsLocked = true;

    private void Start()
    {
        lockScreen.SetActive(true);
        CheckLockedItems();
        GameEvents.Instance.BuyItem.AddListener(CheckLockedItems);
    }

    public void SetInfo(PlantSO plant)
    {
        labelName.text = plant.itemName;
        labelPrice.text = "Price: "+plant.itemPrice;
        previewImage.sprite = plant.grownSprite;
        _currentPlant = plant;
        unlockCoinsRequirement.text = plant.unlockCoins + " Coins";
        _unlockCoins = plant.unlockCoins;
    }

    private void CheckLockedItems()
    {
        if(ShopManager.Instance.CurrentCoins >= _unlockCoins)
        {
            plantButtonIsLocked = false;
            lockScreen.SetActive(false);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (plantButtonIsLocked)
        {
            GameEvents.Instance.Error.Invoke();
            return;
        }
        Instantiate(plant).SetInfo(_currentPlant);
        FindFirstObjectByType<SimpleSideMenu>().Close();
    }
}
