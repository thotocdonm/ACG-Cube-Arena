using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item Data", order = 0)]
public class ItemDataSO : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public Sprite icon;
    public EquipmentType equipmentType;
    public int price;

    [Header("Stats")]
    public List<ItemStatModifier> modifiers;
}

[System.Serializable]
public class ItemStatModifier
{
    public StatType statType;
    public float value;
    public StatModifierType type;
}
