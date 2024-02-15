using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private int currentCoins;
    [SerializeField] private int startCoins;

    private void Start()
    {
        currentCoins = startCoins;
        UpdateCoinText();
    }

    public bool TryToBuySomething(int price)
    {
        if(currentCoins - price >= 0) 
        {
            currentCoins -= price;
            UpdateCoinText();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + currentCoins;
    }
}
