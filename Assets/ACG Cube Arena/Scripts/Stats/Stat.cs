using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stat
{
    private readonly int baseValue;
    private readonly List<StatModifier> modifiers;

    public Stat(int baseValue)
    {
        this.baseValue = baseValue;
        modifiers = new List<StatModifier>();
    }

    public int GetValue()
    {
        float finalValue = baseValue;
        float percentSum = 0;

        foreach (StatModifier mod in modifiers)
        {
            if (mod.type == StatModifierType.Flat)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModifierType.Percentage)
            {
                percentSum += mod.value;
            }
        }

        finalValue *= 1 + percentSum;

        return (int)finalValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
    }
    
    public void RemoveModifier(StatModifier modifier)
    {
        modifiers.Remove(modifier);
    }
}
