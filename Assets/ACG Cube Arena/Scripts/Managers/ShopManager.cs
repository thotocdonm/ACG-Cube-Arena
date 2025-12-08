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
    
    [Header("Shop Rarity Settings")]
    [SerializeField, Range(0, 100)] private int commonItemChance = 50;
    [SerializeField, Range(0, 100)] private int uncommonItemChance = 30;
    [SerializeField, Range(0, 100)] private int rareItemChance = 15;
    [SerializeField, Range(0, 100)] private int epicItemChance = 5;

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
            rerollButton.interactable = CurrencyManager.instance.IsEnoughCoins(rerollPrice);
        }
    }

    public void GenerateItems()
    {
        currentItems.Clear();



        for (int i = 0; i < 3; i++)
        {
            ItemDataSO item = null;
            int safetyCounter = 0;

            while(item == null && safetyCounter < 10)
            {
                safetyCounter++;
                EquipmentRarity rarity = GetRandomRarity();
                List<ItemDataSO> possibleItemsForRarity = possibleItems.FindAll(item => item.rarity == rarity);
                if(possibleItemsForRarity.Count == 0)
                {
                    continue;
                }
                item = possibleItemsForRarity[Random.Range(0, possibleItemsForRarity.Count)];
            }

            if(item != null)
            {
                currentItems.Add(item);
            }
        }
        onItemsGenerated?.Invoke(currentItems);
    }

    public EquipmentRarity GetRandomRarity()
    {
        int r = Random.Range(0, 100);

        if ((r -= commonItemChance) < 0)
            return EquipmentRarity.Common;

        if ((r -= uncommonItemChance) < 0)
            return EquipmentRarity.Uncommon;

        if ((r -= rareItemChance) < 0)
            return EquipmentRarity.Rare;

        if ((r -= epicItemChance) < 0)
            return EquipmentRarity.Epic;

        return EquipmentRarity.Legendary;
    }

    public void RerollShop()
    {
        if (CurrencyManager.instance.IsEnoughCoins(rerollPrice))
        {
            CurrencyManager.instance.RemoveCoins(rerollPrice);
            GenerateItems();
        }
        else
        {
            Debug.Log("Not enough coins");
        }
        
    }
    



}
