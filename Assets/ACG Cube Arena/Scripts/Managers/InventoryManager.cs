using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;

    private Dictionary<EquipmentType, ItemDataSO> equippedItems = new Dictionary<EquipmentType, ItemDataSO>();
    private PlayerStats playerStats;

    [SerializeField] private ItemDataSO testItem;
    [SerializeField] private ItemDataSO testItem2;

    public static Action<EquipmentType, ItemDataSO> onEquipmentChanged;



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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
        else
        {
            Debug.LogError("Player not found");
        }

        equippedItems.Add(EquipmentType.Weapon, null);
        equippedItems.Add(EquipmentType.Armor, null);
        equippedItems.Add(EquipmentType.Boots, null);
        equippedItems.Add(EquipmentType.Accessory, null);
    }

    public void EquipItem(ItemDataSO item)
    {
        EquipmentType slot = item.equipmentType;
        if (equippedItems[slot] != null)
        {
            UnequipItem(slot);
        }
        
        equippedItems[slot] = item;
        foreach (ItemStatModifier itemStatModifier in item.modifiers)
        {
            StatModifier modifier = new StatModifier(itemStatModifier.value, itemStatModifier.type, item);
            playerStats.GetStat(itemStatModifier.statType).AddModifier(modifier);
        }

        onEquipmentChanged?.Invoke(slot, item);
    }

    public void UnequipItem(EquipmentType slot)
    {
        ItemDataSO oldItem = equippedItems[slot];
        if (oldItem == null) return;

        foreach (ItemStatModifier itemStatModifier in oldItem.modifiers)
        {
            playerStats.GetStat(itemStatModifier.statType).RemoveAllModifiersFromSource(oldItem);
        }

        equippedItems[slot] = null;

        CurrencyManager.instance.AddCoins(oldItem.price / 2);

        onEquipmentChanged?.Invoke(slot, null);
    }


    [NaughtyAttributes.Button]
    public void TestEquipItem()
    {
        EquipItem(testItem);
    }

    [NaughtyAttributes.Button]
    public void TestEquipItem2()
    {
        EquipItem(testItem2);
    }

    [NaughtyAttributes.Button]
    public void TestUnequipItem()
    {
        UnequipItem(EquipmentType.Weapon);
    }
    
}
