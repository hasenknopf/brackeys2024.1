using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private TMP_Text coinText;
    private int currentCoins;
    private int currentPots = 0;
    public int CurrentCoins => currentCoins;
    public int CurrentPots => currentPots;
    [SerializeField] private int startCoins;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

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

    public void BuyPot()
    {
        currentPots++;
    }

    public void AddCoins(int price)
    {
        currentCoins += price;
        GameEvents.Instance.BuyItem.Invoke();
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = currentCoins.ToString();
    }
}
