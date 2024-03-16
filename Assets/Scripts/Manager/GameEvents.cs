using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

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

    public UnityEvent Error;
    public UnityEvent Confirm;
    public UnityEvent Pause;

    public UnityEvent BuyItem;
    public UnityEvent SellPlant;

    public UnityEvent SowPlant;
    public UnityEvent WaterPlant;

    public UnityEvent DropPot;
    public UnityEvent DragPot;

    public UnityEvent NewDay;
}
