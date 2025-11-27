using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stat Tooltip", menuName = "Stat/Stat Tooltip", order = 0)]
public class StatTooltipSO : ScriptableObject
{
    public StatType statType;
    public string title;
    [TextArea]
    public string description;
}
