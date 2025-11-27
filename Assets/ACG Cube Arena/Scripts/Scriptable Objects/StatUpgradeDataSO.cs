using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Upgrade Data", menuName = "Stat/Stat Upgrade Data", order = 0)]
public class StatUpgradeDataSO : ScriptableObject
{
    [Header("Link to Data")]
    public StatType statType;
    public StatTooltipSO statTooltip;

    [Header("Upgrade Data")]
    public int maxLevel;
    public float valuePerLevel;
    public StatModifierType modifierType;
    public int baseCost;
    public int costIncreasePerLevel;
}
