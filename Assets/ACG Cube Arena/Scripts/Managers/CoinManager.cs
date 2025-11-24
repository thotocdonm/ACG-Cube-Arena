using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    private int currentCoins = 0;
    public static Action<int> onCoinsChanged;


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

    public bool IsEnoughCoins(int amount)
    {
        return currentCoins >= amount;
    }

    [NaughtyAttributes.Button]
    public void Add100Coins()
    {
        AddCoins(100);
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }
}
