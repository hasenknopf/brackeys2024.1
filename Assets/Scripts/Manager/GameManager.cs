using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

        //Quality Settings
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }


    public int dayCounter = 1;
    [SerializeField] TMP_Text dayCounterText;
    [SerializeField] Animator animator;
    [SerializeField] int targetFrameRate = 60;

    private void Start()
    {
        UpdateDayCounterText();
    }

    public void NextDay()
    {
        GameEvents.Instance.NewDay.Invoke();

        StartCoroutine(LoadNewDay());
        dayCounter++;
        
    }

    private void UpdateDayCounterText()
    {
        dayCounterText.text = "Day: " + dayCounter;
    }

    IEnumerator LoadNewDay()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);
        UpdateDayCounterText();
        Pot[] pots = FindObjectsOfType<Pot>();
        foreach (Pot pot in pots)
        {
            pot.ChangeDay();
        }
    }
}
