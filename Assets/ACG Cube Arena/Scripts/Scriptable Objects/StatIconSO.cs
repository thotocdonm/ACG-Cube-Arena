using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Icon Config", menuName = "Stat Icon/Stat Icon Config", order = 0)]
public class StatIconSO : ScriptableObject
{
    public List<StatIcon> statIcons;

    private Dictionary<StatType, Sprite> statIconMap;

    private void OnEnable()
    {
        statIconMap = new Dictionary<StatType, Sprite>();
        foreach (var icon in statIcons)
        {
            statIconMap.Add(icon.statType, icon.icon);
        }
    }

    public Sprite GetIcon(StatType statType)
    {
        if(statIconMap.TryGetValue(statType, out Sprite icon))
        {
            return icon;
        }
        return null;
    }
}


[System.Serializable]
public struct StatIcon
{
    public StatType statType;
    public Sprite icon;
}
