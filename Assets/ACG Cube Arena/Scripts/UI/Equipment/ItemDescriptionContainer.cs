using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemBorder;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private TextMeshProUGUI recycleButtonText;
    private ItemDataSO itemData;

    [Header("Test")]
    [SerializeField] ItemDataSO testItem;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Configure(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemImage.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemName.color = ResourcesManager.instance.GetRarityColor(itemData.rarity);
        itemBorder.color = ResourcesManager.instance.GetRarityColor(itemData.rarity);
        itemDescription.text = itemData.description;
        if(buyButtonText != null)
        {
            buyButtonText.text = $"Buy (-{itemData.price})";
        }
        if(recycleButtonText != null)
        {
            recycleButtonText.text = $"Recycle (+{itemData.price / 2})";
        }
    }

    public void BuyItem()
    {
        if(itemData != null)
        {
            InventoryManager.instance.EquipItem(itemData);
            ShopManager.instance.CloseShop();
        }
    }


}
