using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySectionUI : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private ItemSlotUI weaponSlot;
    [SerializeField] private ItemSlotUI armorSlot;
    [SerializeField] private ItemSlotUI bootsSlot;
    [SerializeField] private ItemSlotUI accessorySlot1;

    private Dictionary<EquipmentType, ItemSlotUI> equipmentSlots;

    // Start is called before the first frame update
    void Start()
    {
        equipmentSlots = new Dictionary<EquipmentType, ItemSlotUI>
        {
            { EquipmentType.Weapon, weaponSlot },
            { EquipmentType.Armor, armorSlot },
            { EquipmentType.Boots, bootsSlot },
            { EquipmentType.Accessory, accessorySlot1 },
        };

        InventoryManager.onEquipmentChanged += OnEquipmentChangedCallback;
        
    }
    
    void OnDestroy()
    {
        InventoryManager.onEquipmentChanged -= OnEquipmentChangedCallback;     
    }

    private void OnEquipmentChangedCallback(EquipmentType slot, ItemDataSO itemData)
    {
        if(equipmentSlots.TryGetValue(slot, out ItemSlotUI slotUI))
        {
            slotUI.UpdateSlot(itemData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
