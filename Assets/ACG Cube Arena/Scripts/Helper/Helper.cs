using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper 
{
    public static float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}
