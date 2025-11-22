using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetailPanelUI : MonoBehaviour
{
    public static ItemDetailPanelUI instance;

    [Header("Elements")]
    [SerializeField] private ItemDescriptionContainer itemDescriptionContainer;
    
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
    
    public void Configure(ItemDataSO itemData)
    {
        itemDescriptionContainer.Configure(itemData);
    }
}
