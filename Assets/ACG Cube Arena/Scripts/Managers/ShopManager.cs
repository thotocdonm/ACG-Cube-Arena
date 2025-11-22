using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("Elements")]
    [SerializeField] private List<ItemDataSO> possibleItems;

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

    public void OpenShop()
    {
        GameUIManager.instance.ShowShopPanel();
        GenerateItems();
        GameStateManager.instance.ChangeGameState(GameState.Shopping);

    }

    public void CloseShop()
    {
        GameUIManager.instance.HideShopPanel();
        GameStateManager.instance.ChangeGameState(GameState.Game);
    }

    public void RerollShop()
    {
        GenerateItems();
    }
    



}
