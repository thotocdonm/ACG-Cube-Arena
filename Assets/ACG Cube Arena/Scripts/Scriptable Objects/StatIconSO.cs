using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatIconSO : ScriptableObject
{
    public List<StatIcon> statIcons;
}


[System.Serializable]
public struct StatIcon
{
    public StatType statType;
    public Sprite icon;
}
