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
    private float _minValue;
    private float _maxValue;

    public Stat(float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        this.baseValue = baseValue;
        modifiers = new List<StatModifier>();
        Modifiers = modifiers.AsReadOnly();
        _lastValue = GetValue();
        _minValue = minValue;
        _maxValue = maxValue;
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

        finalValue *= 1 + percentSum / 100;

        finalValue = Mathf.Clamp(finalValue, _minValue, _maxValue);
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

    public void RemoveAllModifiersFromSource(object source)
    {
        for(int i = modifiers.Count - 1; i >= 0; i--)
        {
            if(modifiers[i].source == source)
            {
                modifiers.RemoveAt(i);
            }
        }
        CheckForChange();
    }
    
    private void CheckForChange()
    {
        float newValue = GetValue();
        if (newValue != _lastValue)
        {
            Debug.Log("Stat value changed: " + _lastValue + " -> " + newValue);
            _lastValue = newValue;
            OnValueChanged?.Invoke(newValue);
        }
    }
}
