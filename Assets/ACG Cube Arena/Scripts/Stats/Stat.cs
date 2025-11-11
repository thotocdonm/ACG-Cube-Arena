using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


[System.Serializable]
public class Stat
{
    public readonly int baseValue;
    private readonly List<StatModifier> modifiers;
    public readonly ReadOnlyCollection<StatModifier> Modifiers;

    public event Action<int> OnValueChanged;

    private int _lastValue;

    public Stat(int baseValue)
    {
        this.baseValue = baseValue;
        modifiers = new List<StatModifier>();
        Modifiers = modifiers.AsReadOnly();
        _lastValue = GetValue();
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
        CheckForChange();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        modifiers.Remove(modifier);
        CheckForChange();
    }
    
    private void CheckForChange()
    {
        int newValue = GetValue();
        if (newValue != _lastValue)
        {
            _lastValue = newValue;
            OnValueChanged?.Invoke(newValue);
        }
    }
}
