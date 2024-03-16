using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellPlantHolder : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text priceText;
    [SerializeField] GameObject sellMenu;
    [SerializeField] List<PotHolder> potHolders;
    private Pot currentPot;

    private void Start()
    {
        sellMenu.SetActive(false);
    }

    public void SetInfo(string name, int price)
    {
        nameText.text = name;
        priceText.text = "Sell for: " + price; 
    }

    public void SetPot(Pot pot)
    {
        currentPot = pot;
    }

    public void OpenMenu()
    {
        sellMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        sellMenu.SetActive(false);
    }

    public void SellPlant()
    {
        currentPot.SellPlantInPot();
        StorePot();

        SetPot(null);
        GetComponent<PotHolder>().isHolderEmpty = true;
    }

    private void StorePot()
    {
        int i = 0;
        while (!potHolders[i].isHolderEmpty)
        {
            i++;
        }
        currentPot.SetNewPotHolder(potHolders[i]);
    }
}
