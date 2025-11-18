using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


[System.Serializable]
public class Stat
{
    public readonly float baseValue;
    private readonly List<StatModifier> modifiers;
    public readonly ReadOnlyCollection<StatModifier> Modifiers;

    public event Action<float> OnValueChanged;

    private float _lastValue;

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
        modifiers = new List<StatModifier>();
        Modifiers = modifiers.AsReadOnly();
        _lastValue = GetValue();
    }

    public float GetValue()
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

        return finalValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        for(int i = 0; i < modifiers.Count; i++)
        {
            if(modifiers[i].source == modifier.source)
            {
                modifiers.RemoveAt(i);
            }
        }

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
        float newValue = GetValue();
        if (newValue != _lastValue)
        {
            _lastValue = newValue;
            OnValueChanged?.Invoke(newValue);
        }
    }
}
