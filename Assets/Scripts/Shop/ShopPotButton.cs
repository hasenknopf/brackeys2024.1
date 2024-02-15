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
    [SerializeField] Image previewImage;
    [SerializeField] Pot potObject;
    private PotSO currentPot;

    public void SetInfo(PotSO pot)
    {
        labelName.text = pot.itemName;
        labelPrice.text = "Price: " + pot.itemPrice;
        previewImage.sprite = pot.potSprite;
        currentPot = pot;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Instantiate(potObject).SetPot(currentPot);
        FindFirstObjectByType<SimpleSideMenu>().Close();
    }
}
