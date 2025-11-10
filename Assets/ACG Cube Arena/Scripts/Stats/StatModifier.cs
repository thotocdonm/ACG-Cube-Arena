using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatModifierType
{
    Flat,
    Percentage
}

public class StatModifier
{
    public readonly float value;
    public readonly StatModifierType type;

    public StatModifier(float value, StatModifierType type)
    {
        this.value = value;
        this.type = type;
    }
}
