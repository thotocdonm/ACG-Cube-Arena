using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stat
{
    private int baseValue;

    public Stat(int baseValue)
    {
        this.baseValue = baseValue;
    }

    public int GetValue()
    {
        return baseValue;
    }
}
