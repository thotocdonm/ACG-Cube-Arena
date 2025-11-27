using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    public static string SplitCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return System.Text.RegularExpressions.Regex
            .Replace(input, "([a-z])([A-Z])", "$1 $2");
    }
}

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, GameObject obj)
    {
        return (mask & (1 << obj.layer)) != 0;
    }
}
