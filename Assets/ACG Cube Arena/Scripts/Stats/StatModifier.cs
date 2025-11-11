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
    public readonly object source;

    public StatModifier(float value, StatModifierType type, object source)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }
}
