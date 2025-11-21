using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityColorManager : MonoBehaviour
{
    public static RarityColorManager instance;

    [Header("Elements")]
    [SerializeField] private RarityColorConfigSO rarityColorConfig;

    private void Awake()
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

    public Color GetColor(EquipmentRarity rarity)
    {
        return rarityColorConfig.GetColor(rarity);
    }
}
