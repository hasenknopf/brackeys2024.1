using DanielLochner.Assets.SimpleSideMenu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPotButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TMP_Text labelName;
    [SerializeField] TMP_Text labelPrice;
    [SerializeField] GameObject lockScreen;
    [SerializeField] Image previewImage;
    [SerializeField] Pot potObject;
    private int _maxPots;
    private PotSO currentPot;

    public bool potButtonIsLocked = false;

    private void Start()
    {
        lockScreen.SetActive(false);
        CheckLockedItems();
        GameEvents.Instance.BuyItem.AddListener(CheckLockedItems);
    }

    public void SetInfo(PotSO pot)
    {
        labelName.text = pot.itemName;
        labelPrice.text = "Price: " + pot.itemPrice;
        previewImage.sprite = pot.potSprite;
        _maxPots = pot.maxPots;
        currentPot = pot;
    }

    private void CheckLockedItems()
    {
        if (ShopManager.Instance.CurrentPots >= _maxPots)
        {
            potButtonIsLocked = true;
            lockScreen.SetActive(true);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (potButtonIsLocked)
        {
            GameEvents.Instance.Error.Invoke();
            return;
        }
        Instantiate(potObject).SetPot(currentPot);
        FindFirstObjectByType<SimpleSideMenu>().Close();
    }
}
