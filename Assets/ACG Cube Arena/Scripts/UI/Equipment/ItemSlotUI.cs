using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Image borderImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        borderImage.color = Color.white;
    }
    
    public void UpdateSlot(ItemDataSO itemData)
    {
        if(itemData != null)
        {
            itemImage.sprite = itemData.icon;
            itemImage.gameObject.SetActive(true);
            borderImage.color = RarityColorManager.instance.GetColor(itemData.rarity);
        }
        else
        {
            ClearSlot();
        }
    }
}
