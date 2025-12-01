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

    public static Bounds CalculateBoundsFromColliders(Transform parent)
    {
        Bounds totalBounds = new Bounds();
        bool initialized = false;

        BoxCollider[] colliders = parent.GetComponentsInChildren<BoxCollider>();

        foreach (var col in colliders)
        {
            if (!initialized)
            {
                totalBounds = col.bounds;
                initialized = true;
            }
            else
            {
                totalBounds.Encapsulate(col.bounds);
            }
        }

        return totalBounds;
    }
}

public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask mask, GameObject obj)
    {
        return (mask & (1 << obj.layer)) != 0;
    }
}
