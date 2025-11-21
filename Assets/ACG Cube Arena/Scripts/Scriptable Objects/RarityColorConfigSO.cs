using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity Color Config", menuName = "Rarity/Rarity Color Config", order = 0)]
public class RarityColorConfigSO : ScriptableObject
{
    [System.Serializable]
    public struct RarityColorConfig
    {
        public EquipmentRarity rarity;
        public Color color;
    }

    public List<RarityColorConfig> rarityColorConfigs;

    private Dictionary<EquipmentRarity, Color> rarityColorMap;

    private void OnEnable()
    {
        rarityColorMap = new Dictionary<EquipmentRarity, Color>();
        foreach (var config in rarityColorConfigs)
        {
            rarityColorMap.Add(config.rarity, config.color);
        }
    }
    
    public Color GetColor(EquipmentRarity rarity)
    {
        if(rarityColorMap.TryGetValue(rarity, out Color color))
        {
            return color;
        }
        return Color.white;
    }
}
