using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("Elements")]
    [SerializeField] private List<ItemDataSO> possibleItems;
    [SerializeField] private Button rerollButton;

    [Header("Price Settings")]
    [SerializeField] private int rerollPrice = 30;

    private List<ItemDataSO> currentItems = new List<ItemDataSO>();

    public static Action<List<ItemDataSO>> onItemsGenerated;

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

    void Update()
    {
        if (rerollButton != null)
        {
            rerollButton.interactable = CoinManager.instance.IsEnoughCoins(rerollPrice);
        }
    }

    public void GenerateItems()
    {
        currentItems.Clear();
        for (int i = 0; i < 3; i++)
        {
            ItemDataSO item = possibleItems[Random.Range(0, possibleItems.Count)];
            currentItems.Add(item);
        }
        onItemsGenerated?.Invoke(currentItems);
    }

    public void RerollShop()
    {
        if (CoinManager.instance.IsEnoughCoins(rerollPrice))
        {
            CoinManager.instance.RemoveCoins(rerollPrice);
            GenerateItems();
        }
        else
        {
            Debug.Log("Not enough coins");
        }
        
    }
    



}
