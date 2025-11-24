using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance;

    [Header("Elements")]
    [SerializeField] private RarityColorConfigSO rarityColorConfig;
    [SerializeField] private StatIconSO statIconConfig;

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

    public Color GetRarityColor(EquipmentRarity rarity)
    {
        return rarityColorConfig.GetColor(rarity);
    }

    public Sprite GetStatIcon(StatType statType)
    {
        return statIconConfig.GetIcon(statType);
    }
}