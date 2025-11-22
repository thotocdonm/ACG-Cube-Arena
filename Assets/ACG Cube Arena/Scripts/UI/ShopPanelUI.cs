using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private List<ItemDescriptionContainer> itemDescriptionContainers;

    void Awake()
    {
        ShopManager.onItemsGenerated += OnItemsGeneratedCallback;
    }
    void Start()
    {
        
    }

    void OnDestroy()
    {
        ShopManager.onItemsGenerated -= OnItemsGeneratedCallback;
    }

    private void OnItemsGeneratedCallback(List<ItemDataSO> items)
    {
        for (int i = 0; i < itemDescriptionContainers.Count; i++)
        {
            itemDescriptionContainers[i].Configure(items[i]);
        }
    }
}
