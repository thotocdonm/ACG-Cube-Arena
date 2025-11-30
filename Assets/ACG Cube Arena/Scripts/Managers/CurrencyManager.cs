using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    private int currentCoins = 0;
    public static Action<int> onCoinsChanged;
    private int currentDiamonds = 0;
    public static Action<int> onDiamondsChanged;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        SaveLoadManager.onDataLoaded += OnDataLoadedCallback;
    }
    void OnDisable()
    {
        SaveLoadManager.onDataLoaded -= OnDataLoadedCallback;
    }

    private void OnDataLoadedCallback(SaveData data)
    {
        SetDiamonds(data.diamonds, false);
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        onCoinsChanged?.Invoke(currentCoins);
    }

    public void RemoveCoins(int amount)
    {
        currentCoins -= amount;
        if (currentCoins < 0)
        {
            currentCoins = 0;
        }
        onCoinsChanged?.Invoke(currentCoins);
    }

    public void AddDiamonds(int amount)
    {
        currentDiamonds += amount;
        onDiamondsChanged?.Invoke(currentDiamonds);
    }

    public void RemoveDiamonds(int amount)
    {
        currentDiamonds -= amount;
        if (currentDiamonds < 0)
        {
            currentDiamonds = 0;
        }
        onDiamondsChanged?.Invoke(currentDiamonds);
    }



    public bool IsEnoughCoins(int amount)
    {
        return currentCoins >= amount;
    }


    public bool IsEnoughDiamonds(int amount)
    {
        return currentDiamonds >= amount;
    }

    [NaughtyAttributes.Button]
    public void Add100Coins()
    {
        AddCoins(100);
    }

    [NaughtyAttributes.Button]
    public void Add1000Diamonds()
    {
        AddDiamonds(1000);
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    public int GetCurrentDiamonds()
    {
        return currentDiamonds;
    }

    public void SetDiamonds(int amount, bool shouldSave = true)
    {
        currentDiamonds = amount;
        if (shouldSave)
        {
            onDiamondsChanged?.Invoke(currentDiamonds);
        }
        
    }
}
